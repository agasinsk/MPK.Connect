using Microsoft.EntityFrameworkCore.Migrations;

namespace MPK.Connect.DataAccess.Migrations
{
    public partial class ShapeBaseForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shapes_ShapeBases_ShapeBaseId",
                table: "Shapes");

            migrationBuilder.DropIndex(
                name: "IX_Shapes_ShapeBaseId",
                table: "Shapes");

            migrationBuilder.DropColumn(
                name: "ShapeBaseId",
                table: "Shapes");

            migrationBuilder.AddForeignKey(
                name: "FK_Shapes_ShapeBases_ShapeId",
                table: "Shapes",
                column: "ShapeId",
                principalTable: "ShapeBases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shapes_ShapeBases_ShapeId",
                table: "Shapes");

            migrationBuilder.AddColumn<string>(
                name: "ShapeBaseId",
                table: "Shapes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shapes_ShapeBaseId",
                table: "Shapes",
                column: "ShapeBaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shapes_ShapeBases_ShapeBaseId",
                table: "Shapes",
                column: "ShapeBaseId",
                principalTable: "ShapeBases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
