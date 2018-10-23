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
    [Migration("20181022124344_ShapeUpdate")]
    partial class ShapeUpdate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MPK.Connect.Model.Agency", b =>
                {
                    b.Property<string>("Name")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<string>("FareUrl");

                    b.Property<string>("Id");

                    b.Property<string>("Language");

                    b.Property<string>("Phone");

                    b.Property<string>("Timezone")
                        .IsRequired();

                    b.Property<string>("Url")
                        .IsRequired();

                    b.HasKey("Name");

                    b.ToTable("Agencies");
                });

            modelBuilder.Entity("MPK.Connect.Model.Calendar", b =>
                {
                    b.Property<string>("ServiceId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("EndDate");

                    b.Property<bool>("Friday");

                    b.Property<bool>("Monday");

                    b.Property<bool>("Saturday");

                    b.Property<DateTime>("StartDate");

                    b.Property<bool>("Sunday");

                    b.Property<bool>("Thursday");

                    b.Property<bool>("Tuesday");

                    b.Property<bool>("Wednesday");

                    b.HasKey("ServiceId");

                    b.ToTable("Calendars");
                });

            modelBuilder.Entity("MPK.Connect.Model.CalendarDate", b =>
                {
                    b.Property<string>("ServiceId");

                    b.Property<DateTime>("Date");

                    b.Property<int>("ExceptionRule");

                    b.HasKey("ServiceId");

                    b.ToTable("CalendarDates");
                });

            modelBuilder.Entity("MPK.Connect.Model.FareAttribute", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AgencyId");

                    b.Property<string>("AgencyName");

                    b.Property<string>("CurrencyType")
                        .IsRequired();

                    b.Property<string>("FareId")
                        .IsRequired();

                    b.Property<int>("PaymentMethod");

                    b.Property<double>("Price");

                    b.Property<int?>("TransferDuration");

                    b.Property<int?>("Transfers")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("AgencyName");

                    b.ToTable("FareAttributes");
                });

            modelBuilder.Entity("MPK.Connect.Model.FareRule", b =>
                {
                    b.Property<string>("FareId");

                    b.Property<string>("ContainsId");

                    b.Property<string>("DestinationId");

                    b.Property<string>("OriginId");

                    b.Property<string>("RouteId");

                    b.HasKey("FareId");

                    b.HasIndex("RouteId");

                    b.ToTable("FareRules");
                });

            modelBuilder.Entity("MPK.Connect.Model.FeedInfo", b =>
                {
                    b.Property<string>("PublisherName")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("EndDate");

                    b.Property<string>("Language")
                        .IsRequired();

                    b.Property<string>("PublisherUrl")
                        .IsRequired();

                    b.Property<DateTime?>("StartDate");

                    b.Property<int?>("Version");

                    b.HasKey("PublisherName");

                    b.ToTable("FeedInfos");
                });

            modelBuilder.Entity("MPK.Connect.Model.Frequency", b =>
                {
                    b.Property<string>("TripId");

                    b.Property<DateTime>("EndTime");

                    b.Property<int>("ExactTimes");

                    b.Property<int>("HeadwaySecs");

                    b.Property<DateTime>("StartTime");

                    b.HasKey("TripId");

                    b.ToTable("Frequencies");
                });

            modelBuilder.Entity("MPK.Connect.Model.Route", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AgencyId");

                    b.Property<string>("AgencyName");

                    b.Property<string>("Color");

                    b.Property<string>("Description");

                    b.Property<string>("LongName")
                        .IsRequired();

                    b.Property<string>("ShortName")
                        .IsRequired();

                    b.Property<string>("SortOrder");

                    b.Property<string>("TextColor");

                    b.Property<int>("Type");

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.HasIndex("AgencyName");

                    b.ToTable("Routes");
                });

            modelBuilder.Entity("MPK.Connect.Model.Shape", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double?>("DistTraveled");

                    b.Property<double>("PointLatitude");

                    b.Property<double>("PointLongitude");

                    b.Property<int>("PointSequence");

                    b.HasKey("Id");

                    b.ToTable("Shapes");
                });

            modelBuilder.Entity("MPK.Connect.Model.Stop", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.Property<string>("Description");

                    b.Property<double>("Latitude");

                    b.Property<int>("LocationType");

                    b.Property<double>("Longitude");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("ParentStation");

                    b.Property<string>("Timezone");

                    b.Property<string>("Url");

                    b.Property<int>("WheelchairBoarding");

                    b.Property<string>("ZoneId");

                    b.HasKey("Id");

                    b.ToTable("Stops");
                });

            modelBuilder.Entity("MPK.Connect.Model.StopTime", b =>
                {
                    b.Property<string>("TripId");

                    b.Property<DateTime>("ArrivalTime");

                    b.Property<DateTime>("DepartureTime");

                    b.Property<int>("DropOffTypes");

                    b.Property<string>("HeadSign");

                    b.Property<int>("PickupType");

                    b.Property<double?>("ShapeDistTraveled");

                    b.Property<string>("StopId")
                        .IsRequired();

                    b.Property<int>("StopSequence");

                    b.Property<int>("TimePoint");

                    b.HasKey("TripId");

                    b.HasIndex("StopId");

                    b.ToTable("StopTimes");
                });

            modelBuilder.Entity("MPK.Connect.Model.Transfer", b =>
                {
                    b.Property<string>("FromStopId");

                    b.Property<string>("ToStopId");

                    b.Property<int?>("MinTransferTime");

                    b.Property<int>("TransferType");

                    b.HasKey("FromStopId", "ToStopId");

                    b.HasIndex("ToStopId");

                    b.ToTable("Transfers");
                });

            modelBuilder.Entity("MPK.Connect.Model.Trip", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BikesAllowed");

                    b.Property<string>("BlockId");

                    b.Property<int?>("DirectionId");

                    b.Property<string>("HeadSign");

                    b.Property<string>("RouteId")
                        .IsRequired();

                    b.Property<string>("ServiceId")
                        .IsRequired();

                    b.Property<string>("ShapeId");

                    b.Property<string>("ShortName");

                    b.Property<int>("WheelchairAccessible");

                    b.HasKey("Id");

                    b.HasIndex("RouteId");

                    b.HasIndex("ServiceId");

                    b.HasIndex("ShapeId");

                    b.ToTable("Trips");
                });

            modelBuilder.Entity("MPK.Connect.Model.CalendarDate", b =>
                {
                    b.HasOne("MPK.Connect.Model.Calendar", "Calendar")
                        .WithMany()
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MPK.Connect.Model.FareAttribute", b =>
                {
                    b.HasOne("MPK.Connect.Model.Agency", "Agency")
                        .WithMany()
                        .HasForeignKey("AgencyName");
                });

            modelBuilder.Entity("MPK.Connect.Model.FareRule", b =>
                {
                    b.HasOne("MPK.Connect.Model.FareAttribute", "FareAttribute")
                        .WithMany()
                        .HasForeignKey("FareId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MPK.Connect.Model.Route", "Route")
                        .WithMany()
                        .HasForeignKey("RouteId");
                });

            modelBuilder.Entity("MPK.Connect.Model.Frequency", b =>
                {
                    b.HasOne("MPK.Connect.Model.Trip", "Trip")
                        .WithMany()
                        .HasForeignKey("TripId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MPK.Connect.Model.Route", b =>
                {
                    b.HasOne("MPK.Connect.Model.Agency", "Agency")
                        .WithMany()
                        .HasForeignKey("AgencyName");
                });

            modelBuilder.Entity("MPK.Connect.Model.StopTime", b =>
                {
                    b.HasOne("MPK.Connect.Model.Stop", "Stop")
                        .WithMany()
                        .HasForeignKey("StopId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MPK.Connect.Model.Trip", "Trip")
                        .WithMany()
                        .HasForeignKey("TripId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MPK.Connect.Model.Transfer", b =>
                {
                    b.HasOne("MPK.Connect.Model.Stop", "FromStop")
                        .WithMany()
                        .HasForeignKey("FromStopId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("MPK.Connect.Model.Stop", "ToStop")
                        .WithMany()
                        .HasForeignKey("ToStopId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("MPK.Connect.Model.Trip", b =>
                {
                    b.HasOne("MPK.Connect.Model.Route", "Route")
                        .WithMany()
                        .HasForeignKey("RouteId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MPK.Connect.Model.Calendar", "Calendar")
                        .WithMany()
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MPK.Connect.Model.Shape", "Shape")
                        .WithMany()
                        .HasForeignKey("ShapeId");
                });
#pragma warning restore 612, 618
        }
    }
}
