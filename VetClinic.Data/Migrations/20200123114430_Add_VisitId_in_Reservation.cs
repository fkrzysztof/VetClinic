using Microsoft.EntityFrameworkCore.Migrations;

namespace VetClinic.Data.Migrations
{
    public partial class Add_VisitId_in_Reservation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VisitId",
                table: "Reservations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_VisitId",
                table: "Reservations",
                column: "VisitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Visits_VisitId",
                table: "Reservations",
                column: "VisitId",
                principalTable: "Visits",
                principalColumn: "VisitID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Visits_VisitId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_VisitId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "VisitId",
                table: "Reservations");
        }
    }
}
