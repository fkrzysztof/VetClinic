using Microsoft.EntityFrameworkCore.Migrations;

namespace VetClinic.Data.Migrations
{
    public partial class removeIsReadedinNewsclass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReaded",
                table: "News");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsReaded",
                table: "News",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
