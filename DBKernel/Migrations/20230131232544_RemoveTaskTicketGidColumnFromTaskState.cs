using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBKernel.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTaskTicketGidColumnFromTaskState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaskTicketGid",
                table: "TaskStates");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TaskTicketGid",
                table: "TaskStates",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
