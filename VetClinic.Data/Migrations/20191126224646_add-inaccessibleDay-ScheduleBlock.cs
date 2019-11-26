using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VetClinic.Data.Migrations
{
    public partial class addinaccessibleDayScheduleBlock : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "Time",
                table: "ScheduleBlocks",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "TimeInterval",
                table: "ScheduleBlocks",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "InaccessibleDays",
                columns: table => new
                {
                    InaccessibleDayID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InaccessibleDays", x => x.InaccessibleDayID);
                });

            migrationBuilder.CreateTable(
                name: "VisitTreatment",
                columns: table => new
                {
                    VisitTreatmentID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VisitID = table.Column<int>(nullable: true),
                    TreatmentID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitTreatment", x => x.VisitTreatmentID);
                    table.ForeignKey(
                        name: "FK_VisitTreatment_Treatments_TreatmentID",
                        column: x => x.TreatmentID,
                        principalTable: "Treatments",
                        principalColumn: "TreatmentID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VisitTreatment_Visits_VisitID",
                        column: x => x.VisitID,
                        principalTable: "Visits",
                        principalColumn: "VisitID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VisitTreatment_TreatmentID",
                table: "VisitTreatment",
                column: "TreatmentID");

            migrationBuilder.CreateIndex(
                name: "IX_VisitTreatment_VisitID",
                table: "VisitTreatment",
                column: "VisitID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InaccessibleDays");

            migrationBuilder.DropTable(
                name: "VisitTreatment");

            migrationBuilder.DropColumn(
                name: "TimeInterval",
                table: "ScheduleBlocks");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Time",
                table: "ScheduleBlocks",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(TimeSpan));
        }
    }
}
