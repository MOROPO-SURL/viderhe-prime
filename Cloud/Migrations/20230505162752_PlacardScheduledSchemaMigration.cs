using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartSolucionesCuba.Viderhe.Cloud.Migrations
{
    public partial class PlacardScheduledSchemaMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "beginat",
                table: "placards",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "endat",
                table: "placards",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "beginat",
                table: "placards");

            migrationBuilder.DropColumn(
                name: "endat",
                table: "placards");
        }
    }
}
