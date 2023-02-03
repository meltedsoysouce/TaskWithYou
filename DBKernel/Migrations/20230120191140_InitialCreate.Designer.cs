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
    [Migration("20230120191140_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.2");

            modelBuilder.Entity("DBKernel.Entity.TaskTicket", b =>
                {
                    b.Property<Guid>("Gid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Detail")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("KigenBi")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("TourokuBi")
                        .HasColumnType("INTEGER");

                    b.HasKey("Gid");

                    b.ToTable("TaskTickets");
                });
#pragma warning restore 612, 618
        }
    }
}