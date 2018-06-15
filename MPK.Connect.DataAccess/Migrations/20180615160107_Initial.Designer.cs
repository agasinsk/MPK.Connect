﻿// <auto-generated />
using System;
using MPK.Connect.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MPK.Connect.DataAccess.Migrations
{
    [DbContext(typeof(MpkContext))]
    [Migration("20180615160107_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.0-rtm-30799")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MPK.Connect.Model.Agency", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Language");

                    b.Property<string>("Name");

                    b.Property<string>("Phone");

                    b.Property<string>("Timezone");

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.ToTable("Agencies");
                });

            modelBuilder.Entity("MPK.Connect.Model.ControlStop", b =>
                {
                    b.Property<int>("StopId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("VariantId");

                    b.HasKey("StopId");

                    b.ToTable("ControlStops");
                });

            modelBuilder.Entity("MPK.Connect.Model.Route", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AgencyId");

                    b.Property<string>("Description");

                    b.Property<string>("LongName");

                    b.Property<int>("RouteTypeId");

                    b.Property<string>("ShortName");

                    b.Property<int>("StopTypeId");

                    b.Property<DateTime>("ValidFrom");

                    b.Property<DateTime>("ValidUntil");

                    b.HasKey("Id");

                    b.ToTable("Routes");
                });

            modelBuilder.Entity("MPK.Connect.Model.RouteType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("RouteTypes");
                });

            modelBuilder.Entity("MPK.Connect.Model.Shape", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Latitude");

                    b.Property<string>("Longitude");

                    b.Property<int>("Sequence");

                    b.HasKey("Id");

                    b.ToTable("Shapes");
                });

            modelBuilder.Entity("MPK.Connect.Model.Stop", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Latitude");

                    b.Property<string>("Longitude");

                    b.Property<string>("Name");

                    b.Property<string>("StopTypeId");

                    b.HasKey("Id");

                    b.HasIndex("StopTypeId");

                    b.ToTable("Stops");
                });

            modelBuilder.Entity("MPK.Connect.Model.StopType", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("StopTypes");
                });

            modelBuilder.Entity("MPK.Connect.Model.Trip", b =>
                {
                    b.Property<string>("TripId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BrigadeId");

                    b.Property<int>("DirectionId");

                    b.Property<int>("RouteId");

                    b.Property<int>("ServiceId");

                    b.Property<int>("ShapeId");

                    b.Property<string>("TripHeadSign");

                    b.Property<int>("Variantid");

                    b.Property<int>("VehicleId");

                    b.HasKey("TripId");

                    b.ToTable("Trips");
                });

            modelBuilder.Entity("MPK.Connect.Model.Variant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DisjoinStopId");

                    b.Property<int>("EquivalentMainVariantId");

                    b.Property<bool>("IsMain");

                    b.Property<int>("JoinStopId");

                    b.HasKey("Id");

                    b.ToTable("Variants");
                });

            modelBuilder.Entity("MPK.Connect.Model.Vehicle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Description");

                    b.Property<int>("Name");

                    b.Property<int>("Symbol");

                    b.HasKey("Id");

                    b.ToTable("Vehicles");
                });

            modelBuilder.Entity("MPK.Connect.Model.Stop", b =>
                {
                    b.HasOne("MPK.Connect.Model.StopType")
                        .WithMany("Stops")
                        .HasForeignKey("StopTypeId");
                });
#pragma warning restore 612, 618
        }
    }
}
