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

            migrationBuilder.DropIndex(
                name: "IX_Reservations_VisitId",
                table: "Reservations");

            migrationBuilder.AddColumn<int>(
                name: "VisitID",
                table: "Reservations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_VisitID",
                table: "Reservations",
                column: "VisitID");

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

            migrationBuilder.DropIndex(
                name: "IX_Reservations_VisitID",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "VisitID",
                table: "Reservations");

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
    }
}
