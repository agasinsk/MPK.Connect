using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace MPK.Connect.DataAccess.Migrations
{
    public partial class ModelsUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WheelchairBording",
                table: "Stops");

            migrationBuilder.AlterColumn<string>(
                name: "ServiceId",
                table: "Trips",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "WheelchairBoarding",
                table: "Stops",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AgencyName",
                table: "Routes",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FareAttributes",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    CurrencyType = table.Column<string>(nullable: false),
                    PaymentMethod = table.Column<int>(nullable: false),
                    Transfers = table.Column<int>(nullable: false),
                    AgencyId = table.Column<string>(nullable: true),
                    TransferDuration = table.Column<long>(nullable: false),
                    AgencyName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FareAttributes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FareAttributes_Agencies_AgencyName",
                        column: x => x.AgencyName,
                        principalTable: "Agencies",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Frequencies",
                columns: table => new
                {
                    TripId = table.Column<string>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    HeadwaySecs = table.Column<long>(nullable: false),
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
                    ArrivalTime = table.Column<DateTime>(nullable: false),
                    DepartureTime = table.Column<DateTime>(nullable: false),
                    StopId = table.Column<string>(nullable: false),
                    StopSequence = table.Column<int>(nullable: false),
                    HeadSign = table.Column<string>(nullable: true),
                    PickupType = table.Column<int>(nullable: false),
                    DropOffTypes = table.Column<int>(nullable: false),
                    ShapeDistTraveled = table.Column<double>(nullable: false),
                    TimePoint = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StopTimes", x => x.TripId);
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

            migrationBuilder.CreateTable(
                name: "Transfers",
                columns: table => new
                {
                    FromStopId = table.Column<string>(nullable: false),
                    ToStopId = table.Column<string>(nullable: false),
                    TransferType = table.Column<int>(nullable: false),
                    MinTransferTime = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transfers", x => new { x.FromStopId, x.ToStopId });
                    table.ForeignKey(
                        name: "FK_Transfers_Stops_FromStopId",
                        column: x => x.FromStopId,
                        principalTable: "Stops",
                        principalColumn: "Id",
                        onUpdate: ReferentialAction.NoAction,
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Transfers_Stops_ToStopId",
                        column: x => x.ToStopId,
                        principalTable: "Stops",
                        principalColumn: "Id",
                        onUpdate: ReferentialAction.NoAction,
                        onDelete: ReferentialAction.NoAction);
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

            migrationBuilder.CreateIndex(
                name: "IX_Trips_ServiceId",
                table: "Trips",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_AgencyName",
                table: "Routes",
                column: "AgencyName");

            migrationBuilder.CreateIndex(
                name: "IX_FareAttributes_AgencyName",
                table: "FareAttributes",
                column: "AgencyName");

            migrationBuilder.CreateIndex(
                name: "IX_FareRules_RouteId",
                table: "FareRules",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_StopTimes_StopId",
                table: "StopTimes",
                column: "StopId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_ToStopId",
                table: "Transfers",
                column: "ToStopId");

            migrationBuilder.AddForeignKey(
                name: "FK_CalendarDates_Calendars_ServiceId",
                table: "CalendarDates",
                column: "ServiceId",
                principalTable: "Calendars",
                principalColumn: "ServiceId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Agencies_AgencyName",
                table: "Routes",
                column: "AgencyName",
                principalTable: "Agencies",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Calendars_ServiceId",
                table: "Trips",
                column: "ServiceId",
                principalTable: "Calendars",
                principalColumn: "ServiceId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalendarDates_Calendars_ServiceId",
                table: "CalendarDates");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Agencies_AgencyName",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Calendars_ServiceId",
                table: "Trips");

            migrationBuilder.DropTable(
                name: "FareRules");

            migrationBuilder.DropTable(
                name: "Frequencies");

            migrationBuilder.DropTable(
                name: "StopTimes");

            migrationBuilder.DropTable(
                name: "Transfers");

            migrationBuilder.DropTable(
                name: "FareAttributes");

            migrationBuilder.DropIndex(
                name: "IX_Trips_ServiceId",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Routes_AgencyName",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "WheelchairBoarding",
                table: "Stops");

            migrationBuilder.DropColumn(
                name: "AgencyName",
                table: "Routes");

            migrationBuilder.AlterColumn<string>(
                name: "ServiceId",
                table: "Trips",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "WheelchairBording",
                table: "Stops",
                nullable: false,
                defaultValue: 0);
        }
    }
}