using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace MPK.Connect.DataAccess.Migrations
{
    public partial class AgencyId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Agencies",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Url = table.Column<string>(nullable: false),
                    Timezone = table.Column<string>(nullable: false),
                    Language = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    FareUrl = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Calendars",

                columns: table => new
                {
                    ServiceId = table.Column<string>(nullable: false),
                    Monday = table.Column<bool>(nullable: false),
                    Tuesday = table.Column<bool>(nullable: false),
                    Wednesday = table.Column<bool>(nullable: false),
                    Thursday = table.Column<bool>(nullable: false),
                    Friday = table.Column<bool>(nullable: false),
                    Saturday = table.Column<bool>(nullable: false),
                    Sunday = table.Column<bool>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calendars", x => x.ServiceId);
                });

            migrationBuilder.CreateTable(
                name: "FeedInfos",
                columns: table => new
                {
                    PublisherName = table.Column<string>(nullable: false),
                    PublisherUrl = table.Column<string>(nullable: false),
                    Language = table.Column<string>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true),
                    Version = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedInfos", x => x.PublisherName);
                });

            migrationBuilder.CreateTable(
                name: "ShapeBases",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShapeBases", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stops",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Latitude = table.Column<double>(nullable: false),
                    Longitude = table.Column<double>(nullable: false),
                    ZoneId = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    LocationType = table.Column<int>(nullable: false),
                    ParentStation = table.Column<string>(nullable: true),
                    Timezone = table.Column<string>(nullable: true),
                    WheelchairBoarding = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stops", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FareAttributes",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    FareId = table.Column<string>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    CurrencyType = table.Column<string>(nullable: false),
                    PaymentMethod = table.Column<int>(nullable: false),
                    Transfers = table.Column<int>(nullable: false),
                    AgencyId = table.Column<string>(nullable: true),
                    TransferDuration = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FareAttributes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FareAttributes_Agencies_AgencyId",
                        column: x => x.AgencyId,
                        principalTable: "Agencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Routes",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AgencyId = table.Column<string>(nullable: true),
                    ShortName = table.Column<string>(nullable: false),
                    LongName = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Url = table.Column<string>(nullable: true),
                    Color = table.Column<string>(nullable: true),
                    TextColor = table.Column<string>(nullable: true),
                    SortOrder = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Routes_Agencies_AgencyId",
                        column: x => x.AgencyId,
                        principalTable: "Agencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CalendarDates",
                columns: table => new
                {
                    ServiceId = table.Column<string>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    ExceptionRule = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarDates", x => x.ServiceId);
                    table.ForeignKey(
                        name: "FK_CalendarDates_Calendars_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Calendars",
                        principalColumn: "ServiceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Shapes",
                columns: table => new
                {
                    ShapeId = table.Column<string>(nullable: false),
                    PointLatitude = table.Column<double>(nullable: false),
                    PointLongitude = table.Column<double>(nullable: false),
                    PointSequence = table.Column<int>(nullable: false),
                    DistTraveled = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shapes", x => new { x.ShapeId, x.PointSequence });
                    table.ForeignKey(
                        name: "FK_Shapes_ShapeBases_ShapeId",
                        column: x => x.ShapeId,
                        principalTable: "ShapeBases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Transfers",
                columns: table => new
                {
                    FromStopId = table.Column<string>(nullable: false),
                    ToStopId = table.Column<string>(nullable: false),
                    TransferType = table.Column<int>(nullable: false),
                    MinTransferTime = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transfers", x => new { x.FromStopId, x.ToStopId });
                    table.ForeignKey(
                        name: "FK_Transfers_Stops_FromStopId",
                        column: x => x.FromStopId,
                        principalTable: "Stops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transfers_Stops_ToStopId",
                        column: x => x.ToStopId,
                        principalTable: "Stops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FareRules",
                columns: table => new
                {
                    FareId = table.Column<string>(nullable: false),
                    RouteId = table.Column<string>(nullable: true),
                    OriginId = table.Column<string>(nullable: true),
                    DestinationId = table.Column<string>(nullable: true),
                    ContainsId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FareRules", x => x.FareId);
                    table.ForeignKey(
                        name: "FK_FareRules_FareAttributes_FareId",
                        column: x => x.FareId,
                        principalTable: "FareAttributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FareRules_Routes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "Routes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Trips",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    RouteId = table.Column<string>(nullable: false),
                    ServiceId = table.Column<string>(nullable: false),
                    HeadSign = table.Column<string>(nullable: true),
                    ShortName = table.Column<string>(nullable: true),
                    DirectionId = table.Column<int>(nullable: true),
                    BlockId = table.Column<string>(nullable: true),
                    ShapeId = table.Column<string>(nullable: true),
                    WheelchairAccessible = table.Column<int>(nullable: false),
                    BikesAllowed = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trips_Routes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "Routes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Trips_Calendars_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Calendars",
                        principalColumn: "ServiceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Trips_ShapeBases_ShapeId",
                        column: x => x.ShapeId,
                        principalTable: "ShapeBases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Frequencies",
                columns: table => new
                {
                    TripId = table.Column<string>(nullable: false),
                    StartTime = table.Column<TimeSpan>(nullable: false),
                    EndTime = table.Column<TimeSpan>(nullable: false),
                    HeadwaySecs = table.Column<int>(nullable: false),
                    ExactTimes = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Frequencies", x => x.TripId);
                    table.ForeignKey(
                        name: "FK_Frequencies_Trips_TripId",
                        column: x => x.TripId,
                        principalTable: "Trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StopTimes",
                columns: table => new
                {
                    TripId = table.Column<string>(nullable: false),
                    ArrivalTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    DepartureTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    StopId = table.Column<string>(nullable: false),
                    StopSequence = table.Column<int>(nullable: false),
                    HeadSign = table.Column<string>(nullable: true),
                    PickupType = table.Column<int>(nullable: false),
                    DropOffTypes = table.Column<int>(nullable: false),
                    ShapeDistTraveled = table.Column<double>(nullable: true),
                    TimePoint = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StopTimes", x => new { x.TripId, x.StopId, x.StopSequence });
                    table.ForeignKey(
                        name: "FK_StopTimes_Stops_StopId",
                        column: x => x.StopId,
                        principalTable: "Stops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StopTimes_Trips_TripId",
                        column: x => x.TripId,
                        principalTable: "Trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FareAttributes_AgencyId",
                table: "FareAttributes",
                column: "AgencyId");

            migrationBuilder.CreateIndex(
                name: "IX_FareRules_RouteId",
                table: "FareRules",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_AgencyId",
                table: "Routes",
                column: "AgencyId");

            migrationBuilder.CreateIndex(
                name: "IX_StopTimes_StopId",
                table: "StopTimes",
                column: "StopId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_ToStopId",
                table: "Transfers",
                column: "ToStopId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_RouteId",
                table: "Trips",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_ServiceId",
                table: "Trips",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_ShapeId",
                table: "Trips",
                column: "ShapeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalendarDates");

            migrationBuilder.DropTable(
                name: "FareRules");

            migrationBuilder.DropTable(
                name: "FeedInfos");

            migrationBuilder.DropTable(
                name: "Frequencies");

            migrationBuilder.DropTable(
                name: "Shapes");

            migrationBuilder.DropTable(
                name: "StopTimes");

            migrationBuilder.DropTable(
                name: "Transfers");

            migrationBuilder.DropTable(
                name: "FareAttributes");

            migrationBuilder.DropTable(
                name: "Trips");

            migrationBuilder.DropTable(
                name: "Stops");

            migrationBuilder.DropTable(
                name: "Routes");

            migrationBuilder.DropTable(
                name: "Calendars");

            migrationBuilder.DropTable(
                name: "ShapeBases");

            migrationBuilder.DropTable(
                name: "Agencies");
        }
    }
}