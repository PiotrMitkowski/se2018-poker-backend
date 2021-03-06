﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using PlanningPoker2018_backend_2.Models;
using System;

namespace PlanningPoker2018_backend_2.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20180521210326_FixTaskRoomFK")]
    partial class FixTaskRoomFK
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PlanningPoker2018_backend_2.Models.ProjectTask", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("RoomId");

                    b.Property<int?>("authorid");

                    b.Property<int>("estimate");

                    b.Property<string>("title")
                        .IsRequired();

                    b.HasKey("id");

                    b.HasIndex("authorid");

                    b.ToTable("ProjectTask");
                });

            modelBuilder.Entity("PlanningPoker2018_backend_2.Models.Room", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("name")
                        .IsRequired();

                    b.HasKey("id");

                    b.ToTable("Room");
                });

            modelBuilder.Entity("PlanningPoker2018_backend_2.Models.User", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("name")
                        .IsRequired();

                    b.Property<int>("roleId")
                        .HasColumnName("roleId");

                    b.HasKey("id");

                    b.HasIndex("roleId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("PlanningPoker2018_backend_2.Models.UserRole", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("name")
                        .IsRequired();

                    b.HasKey("id");

                    b.ToTable("UserRole");
                });

            modelBuilder.Entity("PlanningPoker2018_backend_2.Models.ProjectTask", b =>
                {
                    b.HasOne("PlanningPoker2018_backend_2.Models.User", "author")
                        .WithMany()
                        .HasForeignKey("authorid");
                });

            modelBuilder.Entity("PlanningPoker2018_backend_2.Models.User", b =>
                {
                    b.HasOne("PlanningPoker2018_backend_2.Models.UserRole", "role")
                        .WithMany()
                        .HasForeignKey("roleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
