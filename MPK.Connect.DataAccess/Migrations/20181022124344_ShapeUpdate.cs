using Microsoft.EntityFrameworkCore.Migrations;

namespace MPK.Connect.DataAccess.Migrations
{
    public partial class ShapeUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shapes_Trips_Id",
                table: "Shapes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Shapes",
                table: "Shapes");

            migrationBuilder.DropIndex(
                name: "IX_Shapes_Id",
                table: "Shapes");

            migrationBuilder.AlterColumn<string>(
                name: "ShapeId",
                table: "Trips",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Shapes",
                table: "Shapes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_ShapeId",
                table: "Trips",
                column: "ShapeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Shapes_ShapeId",
                table: "Trips",
                column: "ShapeId",
                principalTable: "Shapes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Shapes_ShapeId",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Trips_ShapeId",
                table: "Trips");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Shapes",
                table: "Shapes");

            migrationBuilder.AlterColumn<string>(
                name: "ShapeId",
                table: "Trips",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Shapes",
                table: "Shapes",
                columns: new[] { "Id", "PointSequence" });

            migrationBuilder.CreateIndex(
                name: "IX_Shapes_Id",
                table: "Shapes",
                column: "Id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Shapes_Trips_Id",
                table: "Shapes",
                column: "Id",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
