using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MPK.Connect.DataAccess.Migrations.SimpleMpk
{
    public partial class InitIntIds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Agencies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Language = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Timezone = table.Column<string>(nullable: false),
                    Url = table.Column<string>(nullable: false),
                    Phone = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    FareUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Calendars",
                columns: table => new
                {
                    ServiceId = table.Column<int>(nullable: false),
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
                name: "Stops",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
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
                name: "Routes",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AgencyId = table.Column<int>(nullable: true),
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
                name: "Trips",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    RouteId = table.Column<string>(nullable: false),
                    ServiceId = table.Column<int>(nullable: false),
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
                });

            migrationBuilder.CreateTable(
                name: "StopTimes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    TripId = table.Column<int>(nullable: false),
                    ArrivalTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    DepartureTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    StopId = table.Column<int>(nullable: false),
                    StopSequence = table.Column<int>(nullable: false),
                    HeadSign = table.Column<string>(nullable: true),
                    PickupType = table.Column<int>(nullable: false),
                    DropOffTypes = table.Column<int>(nullable: false),
                    ShapeDistTraveled = table.Column<double>(nullable: true),
                    TimePoint = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StopTimes", x => x.Id);
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
                name: "IX_Routes_AgencyId",
                table: "Routes",
                column: "AgencyId");

            migrationBuilder.CreateIndex(
                name: "IX_StopTimes_StopId",
                table: "StopTimes",
                column: "StopId");

            migrationBuilder.CreateIndex(
                name: "IX_StopTimes_TripId",
                table: "StopTimes",
                column: "TripId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_RouteId",
                table: "Trips",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_ServiceId",
                table: "Trips",
                column: "ServiceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StopTimes");

            migrationBuilder.DropTable(
                name: "Stops");

            migrationBuilder.DropTable(
                name: "Trips");

            migrationBuilder.DropTable(
                name: "Routes");

            migrationBuilder.DropTable(
                name: "Calendars");

            migrationBuilder.DropTable(
                name: "Agencies");
        }
    }
}
