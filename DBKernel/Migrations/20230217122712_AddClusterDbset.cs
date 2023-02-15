using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBKernel.Migrations
{
    /// <inheritdoc />
    public partial class AddClusterDbset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Cluster",
                table: "Cluster");

            migrationBuilder.RenameTable(
                name: "Cluster",
                newName: "Clusters");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Clusters",
                table: "Clusters",
                column: "Gid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Clusters",
                table: "Clusters");

            migrationBuilder.RenameTable(
                name: "Clusters",
                newName: "Cluster");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cluster",
                table: "Cluster",
                column: "Gid");
        }
    }
}
