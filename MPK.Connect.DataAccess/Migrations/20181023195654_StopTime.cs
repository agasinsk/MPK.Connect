using Microsoft.EntityFrameworkCore.Migrations;

namespace MPK.Connect.DataAccess.Migrations
{
    public partial class StopTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_StopTimes",
                table: "StopTimes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StopTimes",
                table: "StopTimes",
                columns: new[] { "TripId", "StopId", "StopSequence" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_StopTimes",
                table: "StopTimes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StopTimes",
                table: "StopTimes",
                column: "TripId");
        }
    }
}
