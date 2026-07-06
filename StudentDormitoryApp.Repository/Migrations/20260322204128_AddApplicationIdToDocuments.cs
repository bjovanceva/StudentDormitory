using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentDormitoryApp.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddApplicationIdToDocuments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationId",
                table: "Documents",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documents_ApplicationId",
                table: "Documents",
                column: "ApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Applications_ApplicationId",
                table: "Documents",
                column: "ApplicationId",
                principalTable: "Applications",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Applications_ApplicationId",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Documents_ApplicationId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "ApplicationId",
                table: "Documents");
        }
    }
}