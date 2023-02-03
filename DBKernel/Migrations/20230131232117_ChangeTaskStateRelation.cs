using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBKernel.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTaskStateRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskStates_TaskTickets_TaskTicketGid",
                table: "TaskStates");

            migrationBuilder.DropIndex(
                name: "IX_TaskStates_TaskTicketGid",
                table: "TaskStates");

            migrationBuilder.RenameColumn(
                name: "TaskStateGid",
                table: "TaskTickets",
                newName: "TaskState");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TaskState",
                table: "TaskTickets",
                newName: "TaskStateGid");

            migrationBuilder.CreateIndex(
                name: "IX_TaskStates_TaskTicketGid",
                table: "TaskStates",
                column: "TaskTicketGid",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskStates_TaskTickets_TaskTicketGid",
                table: "TaskStates",
                column: "TaskTicketGid",
                principalTable: "TaskTickets",
                principalColumn: "Gid",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
