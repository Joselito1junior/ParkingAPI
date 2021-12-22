using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ParkingAPI.Migrations
{
    public partial class InitialDataBase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Parking",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ParkingSapceStatus = table.Column<bool>(nullable: false),
                    ParkingStatus = table.Column<int>(nullable: false),
                    LastOccupation = table.Column<DateTime>(nullable: false),
                    LastVacancy = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parking", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Parking");
        }
    }
}
