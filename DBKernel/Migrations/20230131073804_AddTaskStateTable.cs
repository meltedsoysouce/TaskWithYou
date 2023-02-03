using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBKernel.Migrations
{
    /// <inheritdoc />
    public partial class AddTaskStateTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TaskStateGid",
                table: "TaskTickets",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "TaskStates",
                columns: table => new
                {
                    Gid = table.Column<Guid>(type: "TEXT", nullable: false),
                    TaskTicketGid = table.Column<Guid>(type: "TEXT", nullable: false),
                    StateName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskStates", x => x.Gid);
                    table.ForeignKey(
                        name: "FK_TaskStates_TaskTickets_TaskTicketGid",
                        column: x => x.TaskTicketGid,
                        principalTable: "TaskTickets",
                        principalColumn: "Gid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskStates_TaskTicketGid",
                table: "TaskStates",
                column: "TaskTicketGid",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskStates");

            migrationBuilder.DropColumn(
                name: "TaskStateGid",
                table: "TaskTickets");
        }
    }
}
