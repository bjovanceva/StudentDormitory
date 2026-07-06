using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentDormitoryApp.Repository.Migrations
{
    /// <inheritdoc />
    public partial class NewApplicationFieldsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "comment",
                table: "Applications",
                newName: "Comment");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Comment",
                table: "Applications",
                newName: "comment");
        }
    }
}