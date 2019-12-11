using Microsoft.EntityFrameworkCore.Migrations;

namespace VetClinic.Data.Migrations
{
    public partial class new_field_Title_in_News : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "News",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "News");
        }
    }
}
