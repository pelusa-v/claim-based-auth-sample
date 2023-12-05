using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace claim_based_auth_sample.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Noteownerid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Notes",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notes_OwnerId",
                table: "Notes",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_AspNetUsers_OwnerId",
                table: "Notes",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_AspNetUsers_OwnerId",
                table: "Notes");

            migrationBuilder.DropIndex(
                name: "IX_Notes_OwnerId",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Notes");
        }
    }
}
