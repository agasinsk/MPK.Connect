using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MPK.Connect.DataAccess.Migrations
{
    public partial class ModelsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ControlStops_Stops_StopId1",
                table: "ControlStops");

            migrationBuilder.DropForeignKey(
                name: "FK_ControlStops_Variants_VariantId",
                table: "ControlStops");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_RouteTypes_RouteTypeId",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_StopTypes_StopTypeId1",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK_Stops_StopTypes_StopTypeId1",
                table: "Stops");

            migrationBuilder.DropTable(
                name: "StopTypes");

            migrationBuilder.DropIndex(
                name: "IX_Stops_StopTypeId1",
                table: "Stops");

            migrationBuilder.DropIndex(
                name: "IX_Routes_StopTypeId1",
                table: "Routes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RouteTypes",
                table: "RouteTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ControlStops",
                table: "ControlStops");

            migrationBuilder.DropColumn(
                name: "StopTypeId",
                table: "Stops");

            migrationBuilder.DropColumn(
                name: "StopTypeId1",
                table: "Routes");

            migrationBuilder.RenameTable(
                name: "RouteTypes",
                newName: "RouteType");

            migrationBuilder.RenameTable(
                name: "ControlStops",
                newName: "ControlStop");

            migrationBuilder.RenameColumn(
                name: "StopTypeId1",
                table: "Stops",
                newName: "Code");

            migrationBuilder.RenameIndex(
                name: "IX_ControlStops_VariantId",
                table: "ControlStop",
                newName: "IX_ControlStop_VariantId");

            migrationBuilder.RenameIndex(
                name: "IX_ControlStops_StopId1",
                table: "ControlStop",
                newName: "IX_ControlStop_StopId1");

            migrationBuilder.AlterColumn<double>(
                name: "Longitude",
                table: "Stops",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Latitude",
                table: "Stops",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Stops",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StopId",
                table: "Routes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VehicleType",
                table: "Routes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "Agencies",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Timezone",
                table: "Agencies",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Agencies",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RouteType",
                table: "RouteType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ControlStop",
                table: "ControlStop",
                column: "StopId");

            migrationBuilder.CreateTable(
                name: "CalendarDates",
                columns: table => new
                {
                    Date = table.Column<DateTime>(nullable: false),
                    ExceptionRule = table.Column<int>(nullable: false),
                    ServiceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarDates", x => x.ServiceId);
                });

            migrationBuilder.CreateTable(
                name: "Calendars",
                columns: table => new
                {
                    EndDate = table.Column<DateTime>(nullable: false),
                    Friday = table.Column<bool>(nullable: false),
                    Monday = table.Column<bool>(nullable: false),
                    Saturday = table.Column<bool>(nullable: false),
                    ServiceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StartDate = table.Column<DateTime>(nullable: false),
                    Sunday = table.Column<bool>(nullable: false),
                    Thursday = table.Column<bool>(nullable: false),
                    Tuesday = table.Column<bool>(nullable: false),
                    Wednesday = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calendars", x => x.ServiceId);
                });

            migrationBuilder.CreateTable(
                name: "FeedInfos",
                columns: table => new
                {
                    EndDate = table.Column<DateTime>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Language = table.Column<string>(nullable: true),
                    PublisherName = table.Column<string>(nullable: true),
                    PublisherUrl = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StopTime",
                columns: table => new
                {
                    ArrivalTime = table.Column<DateTime>(nullable: false),
                    DepartureTime = table.Column<DateTime>(nullable: false),
                    DropOffType = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PickupType = table.Column<int>(nullable: false),
                    StopId = table.Column<int>(nullable: false),
                    StopSequence = table.Column<int>(nullable: false),
                    TripId1 = table.Column<string>(nullable: true),
                    TripId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StopTime", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StopTime_Stops_StopId",
                        column: x => x.StopId,
                        principalTable: "Stops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StopTime_Trips_TripId1",
                        column: x => x.TripId1,
                        principalTable: "Trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Variants_DisjoinStopId",
                table: "Variants",
                column: "DisjoinStopId");

            migrationBuilder.CreateIndex(
                name: "IX_Variants_JoinStopId",
                table: "Variants",
                column: "JoinStopId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_ServiceId",
                table: "Trips",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_StopId",
                table: "Routes",
                column: "StopId");

            migrationBuilder.CreateIndex(
                name: "IX_StopTime_StopId",
                table: "StopTime",
                column: "StopId");

            migrationBuilder.CreateIndex(
                name: "IX_StopTime_TripId1",
                table: "StopTime",
                column: "TripId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ControlStop_Stops_StopId1",
                table: "ControlStop",
                column: "StopId1",
                principalTable: "Stops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ControlStop_Variants_VariantId",
                table: "ControlStop",
                column: "VariantId",
                principalTable: "Variants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_RouteType_RouteTypeId",
                table: "Routes",
                column: "RouteTypeId",
                principalTable: "RouteType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Stops_StopId",
                table: "Routes",
                column: "StopId",
                principalTable: "Stops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Calendars_ServiceId",
                table: "Trips",
                column: "ServiceId",
                principalTable: "Calendars",
                principalColumn: "ServiceId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_CalendarDates_ServiceId",
                table: "Trips",
                column: "ServiceId",
                principalTable: "CalendarDates",
                principalColumn: "ServiceId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Variants_Stops_DisjoinStopId",
                table: "Variants",
                column: "DisjoinStopId",
                principalTable: "Stops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Variants_Stops_JoinStopId",
                table: "Variants",
                column: "JoinStopId",
                principalTable: "Stops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ControlStop_Stops_StopId1",
                table: "ControlStop");

            migrationBuilder.DropForeignKey(
                name: "FK_ControlStop_Variants_VariantId",
                table: "ControlStop");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_RouteType_RouteTypeId",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Stops_StopId",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Calendars_ServiceId",
                table: "Trips");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_CalendarDates_ServiceId",
                table: "Trips");

            migrationBuilder.DropForeignKey(
                name: "FK_Variants_Stops_DisjoinStopId",
                table: "Variants");

            migrationBuilder.DropForeignKey(
                name: "FK_Variants_Stops_JoinStopId",
                table: "Variants");

            migrationBuilder.DropTable(
                name: "CalendarDates");

            migrationBuilder.DropTable(
                name: "Calendars");

            migrationBuilder.DropTable(
                name: "FeedInfos");

            migrationBuilder.DropTable(
                name: "StopTime");

            migrationBuilder.DropIndex(
                name: "IX_Variants_DisjoinStopId",
                table: "Variants");

            migrationBuilder.DropIndex(
                name: "IX_Variants_JoinStopId",
                table: "Variants");

            migrationBuilder.DropIndex(
                name: "IX_Trips_ServiceId",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Routes_StopId",
                table: "Routes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RouteType",
                table: "RouteType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ControlStop",
                table: "ControlStop");

            migrationBuilder.DropColumn(
                name: "StopId",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "VehicleType",
                table: "Routes");

            migrationBuilder.RenameTable(
                name: "RouteType",
                newName: "RouteTypes");

            migrationBuilder.RenameTable(
                name: "ControlStop",
                newName: "ControlStops");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "Stops",
                newName: "StopTypeId1");

            migrationBuilder.RenameIndex(
                name: "IX_ControlStop_VariantId",
                table: "ControlStops",
                newName: "IX_ControlStops_VariantId");

            migrationBuilder.RenameIndex(
                name: "IX_ControlStop_StopId1",
                table: "ControlStops",
                newName: "IX_ControlStops_StopId1");

            migrationBuilder.AlterColumn<string>(
                name: "Longitude",
                table: "Stops",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<string>(
                name: "Latitude",
                table: "Stops",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<string>(
                name: "StopTypeId1",
                table: "Stops",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StopTypeId",
                table: "Stops",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "StopTypeId1",
                table: "Routes",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "Agencies",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Timezone",
                table: "Agencies",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Agencies",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddPrimaryKey(
                name: "PK_RouteTypes",
                table: "RouteTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ControlStops",
                table: "ControlStops",
                column: "StopId");

            migrationBuilder.CreateTable(
                name: "StopTypes",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StopTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stops_StopTypeId1",
                table: "Stops",
                column: "StopTypeId1");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_StopTypeId1",
                table: "Routes",
                column: "StopTypeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ControlStops_Stops_StopId1",
                table: "ControlStops",
                column: "StopId1",
                principalTable: "Stops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ControlStops_Variants_VariantId",
                table: "ControlStops",
                column: "VariantId",
                principalTable: "Variants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_RouteTypes_RouteTypeId",
                table: "Routes",
                column: "RouteTypeId",
                principalTable: "RouteTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_StopTypes_StopTypeId1",
                table: "Routes",
                column: "StopTypeId1",
                principalTable: "StopTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Stops_StopTypes_StopTypeId1",
                table: "Stops",
                column: "StopTypeId1",
                principalTable: "StopTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
