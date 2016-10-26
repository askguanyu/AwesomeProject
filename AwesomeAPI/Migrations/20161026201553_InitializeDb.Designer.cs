using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using AwesomeAPI.DatabaseContext;

namespace AwesomeAPI.Migrations
{
    [DbContext(typeof(ApiDbContext))]
    [Migration("20161026201553_InitializeDb")]
    partial class InitializeDb
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1");

            modelBuilder.Entity("AwesomeAPI.Models.Activity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ActivityTypeId");

                    b.Property<string>("Details")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<DateTime?>("End");

                    b.Property<DateTime>("InsertedOn")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("DATETIME()");

                    b.Property<DateTime>("Start");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.Property<DateTime>("UpdatedOn")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasDefaultValueSql("DATETIME()");

                    b.HasKey("Id");

                    b.HasIndex("ActivityTypeId");

                    b.ToTable("Activity");
                });

            modelBuilder.Entity("AwesomeAPI.Models.ActivityType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.Property<DateTime>("InsertedOn")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("DATETIME()");

                    b.Property<DateTime>("UpdatedOn")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasDefaultValueSql("DATETIME()");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 255);

                    b.HasKey("Id");

                    b.HasIndex("Description")
                        .IsUnique();

                    b.ToTable("ActivityType");
                });

            modelBuilder.Entity("AwesomeAPI.Models.Activity", b =>
                {
                    b.HasOne("AwesomeAPI.Models.ActivityType", "ActivityType")
                        .WithMany("Activities")
                        .HasForeignKey("ActivityTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
