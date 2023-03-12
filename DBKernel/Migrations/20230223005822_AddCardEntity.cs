using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBKernel.Migrations
{
    /// <inheritdoc />
    public partial class AddCardEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Card",
                table: "TaskTickets",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "TicketCards",
                columns: table => new
                {
                    Gid = table.Column<Guid>(type: "TEXT", nullable: false),
                    XCoordinate = table.Column<float>(type: "REAL", nullable: false),
                    YCorrdinate = table.Column<float>(type: "REAL", nullable: false),
                    TaskTicket = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketCards", x => x.Gid);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TicketCards");

            migrationBuilder.DropColumn(
                name: "Card",
                table: "TaskTickets");
        }
    }
}
