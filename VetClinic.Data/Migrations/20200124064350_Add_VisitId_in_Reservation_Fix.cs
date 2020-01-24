using Microsoft.EntityFrameworkCore.Migrations;

namespace VetClinic.Data.Migrations
{
    public partial class Add_VisitId_in_Reservation_Fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Visits_VisitId",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "VisitId",
                table: "Reservations",
                newName: "VisitID");

            migrationBuilder.RenameIndex(
                name: "IX_Reservations_VisitId",
                table: "Reservations",
                newName: "IX_Reservations_VisitID");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Visits_VisitID",
                table: "Reservations",
                column: "VisitID",
                principalTable: "Visits",
                principalColumn: "VisitID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Visits_VisitID",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "VisitID",
                table: "Reservations",
                newName: "VisitId");

            migrationBuilder.RenameIndex(
                name: "IX_Reservations_VisitID",
                table: "Reservations",
                newName: "IX_Reservations_VisitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Visits_VisitId",
                table: "Reservations",
                column: "VisitId",
                principalTable: "Visits",
                principalColumn: "VisitID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
