using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PapaMobileX.Server.DataSource.Migrations
{
    public partial class Modifiedaccounts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Salt",
                table: "Accounts",
                type: "TEXT",
                maxLength: 16,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_UserName",
                table: "Accounts",
                column: "UserName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Accounts_UserName",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Salt",
                table: "Accounts");
        }
    }
}
