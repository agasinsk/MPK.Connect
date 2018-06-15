using Microsoft.EntityFrameworkCore.Migrations;

namespace MPK.Connect.DataAccess.Migrations
{
    public partial class ForeignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stops_StopTypes_StopTypeId",
                table: "Stops");

            migrationBuilder.DropIndex(
                name: "IX_Stops_StopTypeId",
                table: "Stops");

            migrationBuilder.RenameColumn(
                name: "Variantid",
                table: "Trips",
                newName: "VariantId");

            migrationBuilder.RenameColumn(
                name: "TripHeadSign",
                table: "Trips",
                newName: "RouteId1");

            migrationBuilder.RenameColumn(
                name: "TripId",
                table: "Trips",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "RouteId1",
                table: "Trips",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HeadSign",
                table: "Trips",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "StopTypeId",
                table: "Stops",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StopTypeId1",
                table: "Stops",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StopTypeId1",
                table: "Routes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StopId1",
                table: "ControlStops",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trips_RouteId1",
                table: "Trips",
                column: "RouteId1");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_ShapeId",
                table: "Trips",
                column: "ShapeId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_VariantId",
                table: "Trips",
                column: "VariantId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_VehicleId",
                table: "Trips",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Stops_StopTypeId1",
                table: "Stops",
                column: "StopTypeId1");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_AgencyId",
                table: "Routes",
                column: "AgencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_RouteTypeId",
                table: "Routes",
                column: "RouteTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_StopTypeId1",
                table: "Routes",
                column: "StopTypeId1");

            migrationBuilder.CreateIndex(
                name: "IX_ControlStops_StopId1",
                table: "ControlStops",
                column: "StopId1");

            migrationBuilder.CreateIndex(
                name: "IX_ControlStops_VariantId",
                table: "ControlStops",
                column: "VariantId");

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
                name: "FK_Routes_Agencies_AgencyId",
                table: "Routes",
                column: "AgencyId",
                principalTable: "Agencies",
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

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Routes_RouteId1",
                table: "Trips",
                column: "RouteId1",
                principalTable: "Routes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Shapes_ShapeId",
                table: "Trips",
                column: "ShapeId",
                principalTable: "Shapes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Variants_VariantId",
                table: "Trips",
                column: "VariantId",
                principalTable: "Variants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Vehicles_VehicleId",
                table: "Trips",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ControlStops_Stops_StopId1",
                table: "ControlStops");

            migrationBuilder.DropForeignKey(
                name: "FK_ControlStops_Variants_VariantId",
                table: "ControlStops");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Agencies_AgencyId",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_RouteTypes_RouteTypeId",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_StopTypes_StopTypeId1",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK_Stops_StopTypes_StopTypeId1",
                table: "Stops");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Routes_RouteId1",
                table: "Trips");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Shapes_ShapeId",
                table: "Trips");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Variants_VariantId",
                table: "Trips");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Vehicles_VehicleId",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Trips_RouteId1",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Trips_ShapeId",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Trips_VariantId",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Trips_VehicleId",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Stops_StopTypeId1",
                table: "Stops");

            migrationBuilder.DropIndex(
                name: "IX_Routes_AgencyId",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_Routes_RouteTypeId",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_Routes_StopTypeId1",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_ControlStops_StopId1",
                table: "ControlStops");

            migrationBuilder.DropIndex(
                name: "IX_ControlStops_VariantId",
                table: "ControlStops");

            migrationBuilder.DropColumn(
                name: "HeadSign",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "StopTypeId1",
                table: "Stops");

            migrationBuilder.DropColumn(
                name: "StopTypeId1",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "StopId1",
                table: "ControlStops");

            migrationBuilder.RenameColumn(
                name: "VariantId",
                table: "Trips",
                newName: "Variantid");

            migrationBuilder.RenameColumn(
                name: "RouteId1",
                table: "Trips",
                newName: "TripHeadSign");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Trips",
                newName: "TripId");

            migrationBuilder.AlterColumn<string>(
                name: "TripHeadSign",
                table: "Trips",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StopTypeId",
                table: "Stops",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_Stops_StopTypeId",
                table: "Stops",
                column: "StopTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stops_StopTypes_StopTypeId",
                table: "Stops",
                column: "StopTypeId",
                principalTable: "StopTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
