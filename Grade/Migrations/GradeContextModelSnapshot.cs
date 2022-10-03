﻿// <auto-generated />
using System;
using Grade.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Grade.Migrations
{
    [DbContext(typeof(GradeContext))]
    partial class GradeContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Grade.Models.Apresentation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("PresenterId")
                        .HasColumnType("integer");

                    b.Property<int>("SectionId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("PresenterId");

                    b.HasIndex("SectionId");

                    b.ToTable("Apresentation", (string)null);
                });

            modelBuilder.Entity("Grade.Models.Presenter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int?>("ResourceId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ResourceId");

                    b.ToTable("Presenter", (string)null);
                });

            modelBuilder.Entity("Grade.Models.Resource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("UploadedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Resource", (string)null);
                });

            modelBuilder.Entity("Grade.Models.Section", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("boolean");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int?>("ResourceId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ResourceId");

                    b.ToTable("Section", (string)null);
                });

            modelBuilder.Entity("Grade.Models.LooseSection", b =>
                {
                    b.HasBaseType("Grade.Models.Section");

                    b.Property<DateTime>("EndAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("StartAt")
                        .HasColumnType("timestamp with time zone");

                    b.ToTable("LooseSection", (string)null);
                });

            modelBuilder.Entity("Grade.Models.WeeklySection", b =>
                {
                    b.HasBaseType("Grade.Models.Section");

                    b.Property<TimeOnly>("EndAt")
                        .HasColumnType("time without time zone");

                    b.Property<int>("EndDay")
                        .HasColumnType("integer");

                    b.Property<TimeOnly>("StartAt")
                        .HasColumnType("time without time zone");

                    b.Property<int>("StartDay")
                        .HasColumnType("integer");

                    b.ToTable("WeeklySection", (string)null);
                });

            modelBuilder.Entity("Grade.Models.Apresentation", b =>
                {
                    b.HasOne("Grade.Models.Presenter", "Presenter")
                        .WithMany("Apresentations")
                        .HasForeignKey("PresenterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Grade.Models.Section", "Section")
                        .WithMany("Apresentations")
                        .HasForeignKey("SectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Presenter");

                    b.Navigation("Section");
                });

            modelBuilder.Entity("Grade.Models.Presenter", b =>
                {
                    b.HasOne("Grade.Models.Resource", "Resource")
                        .WithMany("Presenters")
                        .HasForeignKey("ResourceId");

                    b.Navigation("Resource");
                });

            modelBuilder.Entity("Grade.Models.Section", b =>
                {
                    b.HasOne("Grade.Models.Resource", "Resource")
                        .WithMany("Sections")
                        .HasForeignKey("ResourceId");

                    b.Navigation("Resource");
                });

            modelBuilder.Entity("Grade.Models.LooseSection", b =>
                {
                    b.HasOne("Grade.Models.Section", null)
                        .WithOne()
                        .HasForeignKey("Grade.Models.LooseSection", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Grade.Models.WeeklySection", b =>
                {
                    b.HasOne("Grade.Models.Section", null)
                        .WithOne()
                        .HasForeignKey("Grade.Models.WeeklySection", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Grade.Models.Presenter", b =>
                {
                    b.Navigation("Apresentations");
                });

            modelBuilder.Entity("Grade.Models.Resource", b =>
                {
                    b.Navigation("Presenters");

                    b.Navigation("Sections");
                });

            modelBuilder.Entity("Grade.Models.Section", b =>
                {
                    b.Navigation("Apresentations");
                });
#pragma warning restore 612, 618
        }
    }
}
