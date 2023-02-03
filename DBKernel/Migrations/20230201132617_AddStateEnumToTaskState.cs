using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBKernel.Migrations
{
    /// <inheritdoc />
    public partial class AddStateEnumToTaskState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "TaskStates",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "TaskStates");
        }
    }
}
