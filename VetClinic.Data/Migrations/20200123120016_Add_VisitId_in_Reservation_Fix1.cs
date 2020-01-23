using Microsoft.EntityFrameworkCore.Migrations;

namespace VetClinic.Data.Migrations
{
    public partial class Add_VisitId_in_Reservation_Fix1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "VisitId",
                table: "Reservations",
                newName: "VisitID");

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

            migrationBuilder.RenameColumn(
                name: "VisitID",
                table: "Reservations",
                newName: "VisitId");

            migrationBuilder.AddColumn<int>(
                name: "VisitID",
                table: "Reservations",
                type: "int",
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
    }
}
