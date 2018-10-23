using Microsoft.EntityFrameworkCore.Migrations;

namespace MPK.Connect.DataAccess.Migrations
{
    public partial class ShapePointUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShapePoint_Shapes_ShapeId",
                table: "ShapePoint");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShapePoint",
                table: "ShapePoint");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ShapePoint");

            migrationBuilder.RenameTable(
                name: "ShapePoint",
                newName: "ShapePoints");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShapePoints",
                table: "ShapePoints",
                columns: new[] { "ShapeId", "PointSequence" });

            migrationBuilder.AddForeignKey(
                name: "FK_ShapePoints_Shapes_ShapeId",
                table: "ShapePoints",
                column: "ShapeId",
                principalTable: "Shapes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShapePoints_Shapes_ShapeId",
                table: "ShapePoints");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShapePoints",
                table: "ShapePoints");

            migrationBuilder.RenameTable(
                name: "ShapePoints",
                newName: "ShapePoint");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "ShapePoint",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShapePoint",
                table: "ShapePoint",
                columns: new[] { "ShapeId", "PointSequence" });

            migrationBuilder.AddForeignKey(
                name: "FK_ShapePoint_Shapes_ShapeId",
                table: "ShapePoint",
                column: "ShapeId",
                principalTable: "Shapes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
