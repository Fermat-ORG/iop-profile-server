﻿using ProfileServer.Data.Models;
using Iop.Profileserver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ProfileServerProtocol;
using ProfileServer.Network;
using ProfileServer.Utils;

namespace ProfileServer.Data.Repositories
{
  /// <summary>
  /// Repository of profile server neighbors.
  /// </summary>
  public class NeighborRepository : GenericRepository<Neighbor>
  {
    private static NLog.Logger log = NLog.LogManager.GetLogger("ProfileServer.Data.Repositories.NeighborRepository");


    /// <summary>
    /// Creates instance of the repository.
    /// </summary>
    /// <param name="context">Database context.</param>
    public NeighborRepository(Context context)
      : base(context)
    {
    }


    /// <summary>
    /// Deletes neighbor server, all its profiles and all neighborhood actions for it from the database.
    /// </summary>
    /// <param name="UnitOfWork">Unit of work instance.</param>
    /// <param name="NeighborId">Identifier of the neighbor server to delete.</param>
    /// <param name="ActionId">If there is a neighborhood action that should NOT be deleted, this is its ID, otherwise it is -1.</param>
    /// <returns>true if the function succeeds, false otherwise.</returns>
    public async Task<bool> DeleteNeighbor(UnitOfWork UnitOfWork, byte[] NeighborId, int ActionId = -1)
    {
      log.Trace("(NeighborId:'{0}',ActionId:{1})", NeighborId.ToHex(), ActionId);

      bool res = false;
      List<Guid> imagesToDelete = new List<Guid>();
      using (UnitOfWork unitOfWork = new UnitOfWork())
      {
        bool success = false;

        // Disable change tracking for faster multiple deletes.
        unitOfWork.Context.ChangeTracker.AutoDetectChangesEnabled = false;

        // Delete neighbor from the list of neighbors.
        DatabaseLock lockObject = UnitOfWork.NeighborLock;
        await unitOfWork.AcquireLockAsync(lockObject);
        try
        {
          Neighbor neighbor = (await unitOfWork.NeighborRepository.GetAsync(n => n.NeighborId == NeighborId)).FirstOrDefault();
          if (neighbor != null)
          {
            unitOfWork.NeighborRepository.Delete(neighbor);
            await unitOfWork.SaveThrowAsync();
            log.Debug("Neighbor ID '{0}' deleted from database.", NeighborId.ToHex());
          }
          else
          {
            // If the neighbor does not exist, we set success to true as the result of the operation is as we want it 
            // and we gain nothing by trying to repeat the action later.
            log.Warn("Neighbor ID '{0}' not found.", NeighborId.ToHex());
          }

          success = true;
        }
        catch (Exception e)
        {
          log.Error("Exception occurred: {0}", e.ToString());
        }
        unitOfWork.ReleaseLock(lockObject);

        // Delete neighbor's profiles from the database.
        if (success)
        {
          success = false;

          lockObject = UnitOfWork.NeighborIdentityLock;
          await unitOfWork.AcquireLockAsync(lockObject);
          try
          {
            List<NeighborIdentity> identities = (await unitOfWork.NeighborIdentityRepository.GetAsync(i => i.HostingServerId == NeighborId)).ToList();
            if (identities.Count > 0)
            {
              log.Debug("There are {0} identities of removed neighbor ID '{1}'.", identities.Count, NeighborId.ToHex());
              foreach (NeighborIdentity identity in identities)
              {
                if (identity.ProfileImage != null) imagesToDelete.Add(identity.ProfileImage.Value);
                if (identity.ThumbnailImage != null) imagesToDelete.Add(identity.ThumbnailImage.Value);

                unitOfWork.NeighborIdentityRepository.Delete(identity);
              }

              await unitOfWork.SaveThrowAsync();
              log.Debug("{0} identities hosted on neighbor ID '{1}' deleted from database.", identities.Count, NeighborId.ToHex());
            }
            else log.Trace("No profiles hosted on neighbor ID '{0}' found.", NeighborId.ToHex());

            success = true;
          }
          catch (Exception e)
          {
            log.Error("Exception occurred: {0}", e.ToString());
          }

          unitOfWork.ReleaseLock(lockObject);
        }

        if (success)
        {
          success = false;
          lockObject = UnitOfWork.NeighborhoodActionLock;
          await unitOfWork.AcquireLockAsync(lockObject);
          try
          {
            // Do not delete the current action, it will be deleted just after this method finishes.
            List<NeighborhoodAction> actions = unitOfWork.NeighborhoodActionRepository.Get(a => (a.ServerId == NeighborId) && (a.Id != ActionId)).ToList();
            if (actions.Count > 0)
            {
              foreach (NeighborhoodAction action in actions)
              {
                log.Debug("Action ID {0}, type {1}, serverId '{2}' will be removed from the database.", action.Id, action.Type, NeighborId.ToHex());
                unitOfWork.NeighborhoodActionRepository.Delete(action);
              }

              await unitOfWork.SaveThrowAsync();
            }
            else log.Debug("No neighborhood actions for neighbor ID '{0}' found.", NeighborId.ToHex());

            success = true;
          }
          catch (Exception e) 
          {
            log.Error("Exception occurred: {0}", e.ToString());
          }

          unitOfWork.ReleaseLock(lockObject);
        }

        res = success;
      }


      foreach (Guid guid in imagesToDelete)
        if (!ImageHelper.DeleteImageFile(guid))
          log.Warn("Unable to delete image file of image GUID '{0}'.", guid);

      log.Trace("(-):{0}", res);
      return res;
    }
  }
}