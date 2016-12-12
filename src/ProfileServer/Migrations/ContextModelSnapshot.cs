﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using ProfileServer.Data;
using ProfileServer.Data.Models;

namespace ProfileServer.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752");

            modelBuilder.Entity("ProfileServer.Data.Models.Follower", b =>
                {
                    b.Property<byte[]>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(32);

                    b.Property<string>("IpAddress")
                        .IsRequired();

                    b.Property<DateTime?>("LastRefreshTime");

                    b.Property<int>("PrimaryPort");

                    b.Property<int?>("SrNeighborPort");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("LastRefreshTime");

                    b.HasIndex("IpAddress", "PrimaryPort");

                    b.ToTable("Followers");
                });

            modelBuilder.Entity("ProfileServer.Data.Models.HostedIdentity", b =>
                {
                    b.Property<byte[]>("IdentityId")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(32);

                    b.Property<DateTime?>("ExpirationDate");

                    b.Property<string>("ExtraData")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<byte[]>("HostingServerId")
                        .HasMaxLength(32);

                    b.Property<decimal>("InitialLocationLatitude")
                        .HasColumnType("decimal(9,6)");

                    b.Property<decimal>("InitialLocationLongitude")
                        .HasColumnType("decimal(9,6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<Guid?>("ProfileImage");

                    b.Property<byte[]>("PublicKey")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<Guid?>("ThumbnailImage");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<byte[]>("Version")
                        .IsRequired()
                        .HasMaxLength(3);

                    b.HasKey("IdentityId");

                    b.HasIndex("ExpirationDate");

                    b.HasIndex("ExtraData");

                    b.HasIndex("IdentityId")
                        .IsUnique();

                    b.HasIndex("Name");

                    b.HasIndex("Type");

                    b.HasIndex("InitialLocationLatitude", "InitialLocationLongitude");

                    b.HasIndex("ExpirationDate", "InitialLocationLatitude", "InitialLocationLongitude", "Type", "Name");

                    b.ToTable("Identities");
                });

            modelBuilder.Entity("ProfileServer.Data.Models.Neighbor", b =>
                {
                    b.Property<byte[]>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(32);

                    b.Property<string>("IpAddress")
                        .IsRequired();

                    b.Property<DateTime?>("LastRefreshTime");

                    b.Property<decimal>("LocationLatitude")
                        .HasColumnType("decimal(9,6)");

                    b.Property<decimal>("LocationLongitude")
                        .HasColumnType("decimal(9,6)");

                    b.Property<int>("PrimaryPort");

                    b.Property<int?>("SrNeighborPort");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("LastRefreshTime");

                    b.HasIndex("IpAddress", "PrimaryPort");

                    b.ToTable("Neighbors");
                });

            modelBuilder.Entity("ProfileServer.Data.Models.NeighborhoodAction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AdditionalData");

                    b.Property<DateTime?>("ExecuteAfter");

                    b.Property<byte[]>("ServerId")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<byte[]>("TargetIdentityId")
                        .HasMaxLength(32);

                    b.Property<DateTime>("Timestamp");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.HasIndex("ExecuteAfter");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("ServerId");

                    b.HasIndex("TargetIdentityId");

                    b.HasIndex("Timestamp");

                    b.HasIndex("Type");

                    b.HasIndex("ServerId", "Type", "TargetIdentityId");

                    b.ToTable("NeighborhoodActions");
                });

            modelBuilder.Entity("ProfileServer.Data.Models.NeighborIdentity", b =>
                {
                    b.Property<byte[]>("IdentityId")
                        .HasMaxLength(32);

                    b.Property<byte[]>("HostingServerId")
                        .HasMaxLength(32);

                    b.Property<DateTime?>("ExpirationDate");

                    b.Property<string>("ExtraData")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<decimal>("InitialLocationLatitude")
                        .HasColumnType("decimal(9,6)");

                    b.Property<decimal>("InitialLocationLongitude")
                        .HasColumnType("decimal(9,6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<Guid?>("ProfileImage");

                    b.Property<byte[]>("PublicKey")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<Guid?>("ThumbnailImage");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<byte[]>("Version")
                        .IsRequired()
                        .HasMaxLength(3);

                    b.HasKey("IdentityId", "HostingServerId");

                    b.HasIndex("ExpirationDate");

                    b.HasIndex("ExtraData");

                    b.HasIndex("HostingServerId");

                    b.HasIndex("Name");

                    b.HasIndex("Type");

                    b.HasIndex("IdentityId", "HostingServerId")
                        .IsUnique();

                    b.HasIndex("InitialLocationLatitude", "InitialLocationLongitude");

                    b.HasIndex("InitialLocationLatitude", "InitialLocationLongitude", "Type", "Name");

                    b.ToTable("NeighborhoodIdentities");
                });

            modelBuilder.Entity("ProfileServer.Data.Models.RelatedIdentity", b =>
                {
                    b.Property<byte[]>("IdentityId")
                        .HasMaxLength(32);

                    b.Property<byte[]>("ApplicationId")
                        .HasMaxLength(32);

                    b.Property<byte[]>("CardId")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<byte[]>("CardVersion")
                        .IsRequired()
                        .HasMaxLength(3);

                    b.Property<byte[]>("IssuerPublicKey")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<byte[]>("IssuerSignature")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<byte[]>("RecipientPublicKey")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<byte[]>("RecipientSignature")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<byte[]>("RelatedToIdentityId")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<DateTime>("ValidFrom");

                    b.Property<DateTime>("ValidTo");

                    b.HasKey("IdentityId", "ApplicationId");

                    b.HasIndex("RelatedToIdentityId");

                    b.HasIndex("Type");

                    b.HasIndex("IdentityId", "ApplicationId")
                        .IsUnique();

                    b.HasIndex("ValidFrom", "ValidTo");

                    b.HasIndex("IdentityId", "Type", "RelatedToIdentityId", "ValidFrom", "ValidTo");

                    b.ToTable("RelatedIdentities");
                });

            modelBuilder.Entity("ProfileServer.Data.Models.Setting", b =>
                {
                    b.Property<string>("Name")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Value")
                        .IsRequired();

                    b.HasKey("Name");

                    b.ToTable("Settings");
                });
        }
    }
}
