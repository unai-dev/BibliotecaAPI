using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BibliotecaAPI.Migrations
{
    /// <inheritdoc />
    public partial class ComentsUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Coments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Coments_UserId",
                table: "Coments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Coments_AspNetUsers_UserId",
                table: "Coments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coments_AspNetUsers_UserId",
                table: "Coments");

            migrationBuilder.DropIndex(
                name: "IX_Coments_UserId",
                table: "Coments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Coments");
        }
    }
}
