﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebCAP;

namespace WebCAP.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20200108154524_ini")]
    partial class ini
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("WebCAP.AppDbContext+Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("WebCAP.AppDbContext+Published", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Added");

                    b.Property<string>("Content");

                    b.Property<DateTime>("ExpiresAt");

                    b.Property<string>("Name");

                    b.Property<string>("Retries");

                    b.Property<string>("StatuName");

                    b.HasKey("Id");

                    b.ToTable("cap.published");
                });

            modelBuilder.Entity("WebCAP.AppDbContext+Received", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Added");

                    b.Property<string>("Content");

                    b.Property<DateTime>("ExpiresAt");

                    b.Property<string>("Group");

                    b.Property<string>("Name");

                    b.Property<string>("Retries");

                    b.Property<string>("StatuName");

                    b.HasKey("Id");

                    b.ToTable("cap.received");
                });
#pragma warning restore 612, 618
        }
    }
}
