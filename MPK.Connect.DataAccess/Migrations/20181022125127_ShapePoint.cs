using Microsoft.EntityFrameworkCore.Migrations;

namespace MPK.Connect.DataAccess.Migrations
{
    public partial class ShapePoint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DistTraveled",
                table: "Shapes");

            migrationBuilder.DropColumn(
                name: "PointLatitude",
                table: "Shapes");

            migrationBuilder.DropColumn(
                name: "PointLongitude",
                table: "Shapes");

            migrationBuilder.DropColumn(
                name: "PointSequence",
                table: "Shapes");

            migrationBuilder.CreateTable(
                name: "ShapePoint",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ShapeId = table.Column<string>(nullable: false),
                    PointLatitude = table.Column<double>(nullable: false),
                    PointLongitude = table.Column<double>(nullable: false),
                    PointSequence = table.Column<int>(nullable: false),
                    DistTraveled = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShapePoint", x => new { x.ShapeId, x.PointSequence });
                    table.ForeignKey(
                        name: "FK_ShapePoint_Shapes_ShapeId",
                        column: x => x.ShapeId,
                        principalTable: "Shapes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShapePoint");

            migrationBuilder.AddColumn<double>(
                name: "DistTraveled",
                table: "Shapes",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PointLatitude",
                table: "Shapes",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PointLongitude",
                table: "Shapes",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "PointSequence",
                table: "Shapes",
                nullable: false,
                defaultValue: 0);
        }
    }
}
