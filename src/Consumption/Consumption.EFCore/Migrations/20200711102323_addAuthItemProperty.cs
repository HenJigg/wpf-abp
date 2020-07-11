using Microsoft.EntityFrameworkCore.Migrations;

namespace Consumption.EFCore.Migrations
{
    public partial class addAuthItemProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthColor",
                table: "AuthItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AuthKind",
                table: "AuthItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthColor",
                table: "AuthItems");

            migrationBuilder.DropColumn(
                name: "AuthKind",
                table: "AuthItems");
        }
    }
}
