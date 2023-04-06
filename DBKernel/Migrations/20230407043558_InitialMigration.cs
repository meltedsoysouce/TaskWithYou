using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBKernel.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clusters",
                columns: table => new
                {
                    Gid = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Detail = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clusters", x => x.Gid);
                });

            migrationBuilder.CreateTable(
                name: "TaskStates",
                columns: table => new
                {
                    Gid = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    State = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskStates", x => x.Gid);
                });

            migrationBuilder.CreateTable(
                name: "TaskTickets",
                columns: table => new
                {
                    Gid = table.Column<Guid>(type: "TEXT", nullable: false),
                    IsTodayTask = table.Column<bool>(type: "INTEGER", nullable: false),
                    TourokuBi = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    KigenBi = table.Column<int>(type: "INTEGER", nullable: false),
                    Detail = table.Column<string>(type: "TEXT", nullable: false),
                    StateGid = table.Column<Guid>(type: "TEXT", nullable: false),
                    ClusterGid = table.Column<Guid>(type: "TEXT", nullable: false),
                    CardGid = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskTickets", x => x.Gid);
                });

            migrationBuilder.CreateTable(
                name: "TicketCards",
                columns: table => new
                {
                    Gid = table.Column<Guid>(type: "TEXT", nullable: false),
                    XCoordinate = table.Column<float>(type: "REAL", nullable: false),
                    YCoordinate = table.Column<float>(type: "REAL", nullable: false),
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
                name: "Clusters");

            migrationBuilder.DropTable(
                name: "TaskStates");

            migrationBuilder.DropTable(
                name: "TaskTickets");

            migrationBuilder.DropTable(
                name: "TicketCards");
        }
    }
}
