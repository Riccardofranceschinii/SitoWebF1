﻿// <auto-generated />
using F1_Web.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace F1_Web.Migrations
{
    [DbContext(typeof(F1DbContext))]
    partial class F1DbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("F1_Web.Model.Circuit", b =>
                {
                    b.Property<string>("circuitId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("circuitName")
                        .HasColumnType("longtext");

                    b.Property<string>("url")
                        .HasColumnType("longtext");

                    b.HasKey("circuitId");

                    b.ToTable("Circuits");
                });

            modelBuilder.Entity("F1_Web.Model.Constructor", b =>
                {
                    b.Property<string>("constructorId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("name")
                        .HasColumnType("longtext");

                    b.Property<string>("nationality")
                        .HasColumnType("longtext");

                    b.Property<string>("url")
                        .HasColumnType("longtext");

                    b.HasKey("constructorId");

                    b.ToTable("Constructors");
                });

            modelBuilder.Entity("F1_Web.Model.Driver", b =>
                {
                    b.Property<string>("driverId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("dateOfBirth")
                        .HasColumnType("longtext");

                    b.Property<string>("familyName")
                        .HasColumnType("longtext");

                    b.Property<string>("givenName")
                        .HasColumnType("longtext");

                    b.Property<string>("nationality")
                        .HasColumnType("longtext");

                    b.Property<string>("url")
                        .HasColumnType("longtext");

                    b.HasKey("driverId");

                    b.ToTable("Drivers");
                });

            modelBuilder.Entity("F1_Web.Model.Circuit", b =>
                {
                    b.OwnsOne("F1_Web.Model.Location", "Location", b1 =>
                        {
                            b1.Property<string>("circuitId")
                                .HasColumnType("varchar(255)");

                            b1.Property<string>("Long")
                                .HasColumnType("longtext")
                                .HasAnnotation("Relational:JsonPropertyName", "long");

                            b1.Property<string>("country")
                                .HasColumnType("longtext");

                            b1.Property<string>("lat")
                                .HasColumnType("longtext");

                            b1.Property<string>("locality")
                                .HasColumnType("longtext");

                            b1.HasKey("circuitId");

                            b1.ToTable("Locations");

                            b1.WithOwner()
                                .HasForeignKey("circuitId");
                        });

                    b.Navigation("Location");
                });
#pragma warning restore 612, 618
        }
    }
}
