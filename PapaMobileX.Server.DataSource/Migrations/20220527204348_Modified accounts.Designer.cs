﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PapaMobileX.Server.DataSource;

#nullable disable

namespace PapaMobileX.Server.DataSource.Migrations
{
    [DbContext(typeof(CommonContext))]
    [Migration("20220527204348_Modified accounts")]
    partial class Modifiedaccounts
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.5");

            modelBuilder.Entity("PapaMobileX.Server.DataSource.Models.Account", b =>
                {
                    b.Property<Guid>("AccountGuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("TEXT");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("TEXT");

                    b.Property<string>("SecurityStamp")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("AccountGuid");

                    b.HasIndex("UserName")
                        .IsUnique();

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("PapaMobileX.Server.DataSource.Models.Claim", b =>
                {
                    b.Property<int>("ClaimId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<Guid?>("AccountGuid")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ClaimId");

                    b.HasIndex("AccountGuid");

                    b.ToTable("Claims");
                });

            modelBuilder.Entity("PapaMobileX.Server.DataSource.Models.Claim", b =>
                {
                    b.HasOne("PapaMobileX.Server.DataSource.Models.Account", null)
                        .WithMany("Claims")
                        .HasForeignKey("AccountGuid");
                });

            modelBuilder.Entity("PapaMobileX.Server.DataSource.Models.Account", b =>
                {
                    b.Navigation("Claims");
                });
#pragma warning restore 612, 618
        }
    }
}
