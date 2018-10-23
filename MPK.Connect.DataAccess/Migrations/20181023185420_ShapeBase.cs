using Microsoft.EntityFrameworkCore.Migrations;

namespace MPK.Connect.DataAccess.Migrations
{
    public partial class ShapeBase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Shapes_ShapeId",
                table: "Trips");

            migrationBuilder.DropTable(
                name: "ShapePoints");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Shapes",
                table: "Shapes");

            migrationBuilder.AddColumn<string>(
                name: "ShapeId",
                table: "Shapes",
                nullable: false,
                defaultValue: "");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Shapes");

            migrationBuilder.AddColumn<int>(
                name: "PointSequence",
                table: "Shapes",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.AddColumn<string>(
                name: "ShapeBaseId",
                table: "Shapes",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Shapes",
                table: "Shapes",
                columns: new[] { "ShapeId", "PointSequence" });

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

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_ShapeBases_ShapeId",
                table: "Trips",
                column: "ShapeId",
                principalTable: "ShapeBases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shapes_ShapeBases_ShapeBaseId",
                table: "Shapes");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_ShapeBases_ShapeId",
                table: "Trips");

            migrationBuilder.DropTable(
                name: "ShapeBases");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Shapes",
                table: "Shapes");

            migrationBuilder.DropIndex(
                name: "IX_Shapes_ShapeBaseId",
                table: "Shapes");

            migrationBuilder.DropColumn(
                name: "ShapeId",
                table: "Shapes");

            migrationBuilder.DropColumn(
                name: "PointSequence",
                table: "Shapes");

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
                name: "ShapeBaseId",
                table: "Shapes");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "Shapes",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Shapes",
                table: "Shapes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ShapePoints",
                columns: table => new
                {
                    ShapeId = table.Column<string>(nullable: false),
                    PointSequence = table.Column<int>(nullable: false),
                    DistTraveled = table.Column<double>(nullable: true),
                    PointLatitude = table.Column<double>(nullable: false),
                    PointLongitude = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShapePoints", x => new { x.ShapeId, x.PointSequence });
                    table.ForeignKey(
                        name: "FK_ShapePoints_Shapes_ShapeId",
                        column: x => x.ShapeId,
                        principalTable: "Shapes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Shapes_ShapeId",
                table: "Trips",
                column: "ShapeId",
                principalTable: "Shapes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}