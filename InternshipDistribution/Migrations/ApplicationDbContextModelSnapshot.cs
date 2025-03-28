﻿// <auto-generated />
using System;
using InternshipDistribution.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace InternshipDistribution.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("InternshipDistribution.Models.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("InternshipDistribution.Models.DistributionApplication", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("Priority1CompanyId")
                        .HasColumnType("integer");

                    b.Property<int>("Priority1Status")
                        .HasColumnType("integer");

                    b.Property<int?>("Priority2CompanyId")
                        .HasColumnType("integer");

                    b.Property<int>("Priority2Status")
                        .HasColumnType("integer");

                    b.Property<int?>("Priority3CompanyId")
                        .HasColumnType("integer");

                    b.Property<int>("Priority3Status")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<int>("StudentId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("Priority1CompanyId");

                    b.HasIndex("Priority2CompanyId");

                    b.HasIndex("Priority3CompanyId");

                    b.HasIndex("StudentId");

                    b.ToTable("DistributionApplication");
                });

            modelBuilder.Entity("InternshipDistribution.Models.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Fathername")
                        .HasColumnType("text");

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Resume")
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Students");
                });

            modelBuilder.Entity("InternshipDistribution.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<bool>("IsManager")
                        .HasColumnType("boolean");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("InternshipDistribution.Models.DistributionApplication", b =>
                {
                    b.HasOne("InternshipDistribution.Models.Company", "Priority1Company")
                        .WithMany()
                        .HasForeignKey("Priority1CompanyId");

                    b.HasOne("InternshipDistribution.Models.Company", "Priority2Company")
                        .WithMany()
                        .HasForeignKey("Priority2CompanyId");

                    b.HasOne("InternshipDistribution.Models.Company", "Priority3Company")
                        .WithMany()
                        .HasForeignKey("Priority3CompanyId");

                    b.HasOne("InternshipDistribution.Models.Student", "Student")
                        .WithMany("Applications")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Priority1Company");

                    b.Navigation("Priority2Company");

                    b.Navigation("Priority3Company");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("InternshipDistribution.Models.Student", b =>
                {
                    b.HasOne("InternshipDistribution.Models.User", "User")
                        .WithOne("Student")
                        .HasForeignKey("InternshipDistribution.Models.Student", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("InternshipDistribution.Models.Student", b =>
                {
                    b.Navigation("Applications");
                });

            modelBuilder.Entity("InternshipDistribution.Models.User", b =>
                {
                    b.Navigation("Student");
                });
#pragma warning restore 612, 618
        }
    }
}
