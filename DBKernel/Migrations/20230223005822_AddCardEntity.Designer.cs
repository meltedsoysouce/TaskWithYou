﻿// <auto-generated />
using System;
using DBKernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DBKernel.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230223005822_AddCardEntity")]
    partial class AddCardEntity
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.2");

            modelBuilder.Entity("DBKernel.Entity.Cluster", b =>
                {
                    b.Property<Guid>("Gid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Detail")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Gid");

                    b.ToTable("Clusters");
                });

            modelBuilder.Entity("DBKernel.Entity.TaskState", b =>
                {
                    b.Property<Guid>("Gid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("State")
                        .HasColumnType("INTEGER");

                    b.Property<string>("StateName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Gid");

                    b.ToTable("TaskStates");
                });

            modelBuilder.Entity("DBKernel.Entity.TaskTicket", b =>
                {
                    b.Property<Guid>("Gid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("Card")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("Cluster")
                        .HasColumnType("TEXT");

                    b.Property<string>("Detail")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsTodayTask")
                        .HasColumnType("INTEGER");

                    b.Property<int>("KigenBi")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("TaskState")
                        .HasColumnType("TEXT");

                    b.Property<int>("TourokuBi")
                        .HasColumnType("INTEGER");

                    b.HasKey("Gid");

                    b.ToTable("TaskTickets");
                });

            modelBuilder.Entity("DBKernel.Entity.TicketCard", b =>
                {
                    b.Property<Guid>("Gid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("TaskTicket")
                        .HasColumnType("TEXT");

                    b.Property<float>("XCoordinate")
                        .HasColumnType("REAL");

                    b.Property<float>("YCorrdinate")
                        .HasColumnType("REAL");

                    b.HasKey("Gid");

                    b.ToTable("TicketCards");
                });
#pragma warning restore 612, 618
        }
    }
}
