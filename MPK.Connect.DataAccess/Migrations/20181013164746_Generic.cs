using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MPK.Connect.DataAccess.Migrations
{
    public partial class Generic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Agencies",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: true),
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
                    table.PrimaryKey("PK_Agencies", x => x.Name);
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
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    Version = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedInfos", x => x.PublisherName);
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
                });

            migrationBuilder.CreateTable(
                name: "Shapes",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    PointLatitude = table.Column<string>(nullable: false),
                    PointLongitude = table.Column<string>(nullable: false),
                    PointSequence = table.Column<int>(nullable: false),
                    DistTraveled = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shapes", x => x.Id);
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
                    WheelchairBording = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stops", x => x.Id);
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
                    DirectionId = table.Column<int>(nullable: false),
                    BlockId = table.Column<string>(nullable: true),
                    ShapeId = table.Column<string>(nullable: true)
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
                        name: "FK_Trips_Shapes_ShapeId",
                        column: x => x.ShapeId,
                        principalTable: "Shapes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Trips_RouteId",
                table: "Trips",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_ShapeId",
                table: "Trips",
                column: "ShapeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Agencies");

            migrationBuilder.DropTable(
                name: "CalendarDates");

            migrationBuilder.DropTable(
                name: "Calendars");

            migrationBuilder.DropTable(
                name: "FeedInfos");

            migrationBuilder.DropTable(
                name: "Stops");

            migrationBuilder.DropTable(
                name: "Trips");

            migrationBuilder.DropTable(
                name: "Routes");

            migrationBuilder.DropTable(
                name: "Shapes");
        }
    }
}
