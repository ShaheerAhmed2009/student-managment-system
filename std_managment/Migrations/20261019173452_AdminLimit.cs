using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StdManagment.Migrations
{
    /// <inheritdoc />
    public partial class AdminLimit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Students_Phone",
                table: "Students");

            migrationBuilder.CreateIndex(
                name: "IX_Students_Role",
                table: "Students",
                column: "Role",
                unique: true,
                filter: "[Role] = 'Admin'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Students_Role",
                table: "Students");

            migrationBuilder.CreateIndex(
                name: "IX_Students_Phone",
                table: "Students",
                column: "Phone",
                unique: true);
        }
    }
}
