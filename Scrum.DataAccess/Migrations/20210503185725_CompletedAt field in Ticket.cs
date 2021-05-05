using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Scrum.DataAccess.Migrations
{
    public partial class CompletedAtfieldinTicket : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedAt",
                table: "Tickets",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletedAt",
                table: "Tickets");
        }
    }
}
