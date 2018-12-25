using Microsoft.EntityFrameworkCore.Migrations;

namespace MPK.Connect.DataAccess.Migrations.SimpleMpk
{
    public partial class StopTimeid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_StopTimes",
                table: "StopTimes");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "StopTimes",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StopTimes",
                table: "StopTimes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_StopTimes_TripId",
                table: "StopTimes",
                column: "TripId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_StopTimes",
                table: "StopTimes");

            migrationBuilder.DropIndex(
                name: "IX_StopTimes_TripId",
                table: "StopTimes");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "StopTimes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StopTimes",
                table: "StopTimes",
                columns: new[] { "TripId", "StopId", "StopSequence" });
        }
    }
}
