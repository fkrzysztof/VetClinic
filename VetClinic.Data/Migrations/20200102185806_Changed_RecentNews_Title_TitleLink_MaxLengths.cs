using Microsoft.EntityFrameworkCore.Migrations;

namespace VetClinic.Data.Migrations
{
    public partial class Changed_RecentNews_Title_TitleLink_MaxLengths : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "RecentNews",
                maxLength: 135,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(245)",
                oldMaxLength: 245);

            migrationBuilder.AlterColumn<string>(
                name: "LinkTitle",
                table: "RecentNews",
                maxLength: 44,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(48)",
                oldMaxLength: 48);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "RecentNews",
                type: "nvarchar(245)",
                maxLength: 245,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 135);

            migrationBuilder.AlterColumn<string>(
                name: "LinkTitle",
                table: "RecentNews",
                type: "nvarchar(48)",
                maxLength: 48,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 44);
        }
    }
}
