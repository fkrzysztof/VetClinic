using Microsoft.EntityFrameworkCore.Migrations;

namespace VetClinic.Data.Migrations
{
    public partial class replace_VetID_with_PatientID_in_table_Reservation_add_new_foreign_key : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Users_VetID",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_VetID",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "VetID",
                table: "Reservations");

            migrationBuilder.AddColumn<int>(
                name: "PatientID",
                table: "Reservations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_PatientID",
                table: "Reservations",
                column: "PatientID");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Patients_PatientID",
                table: "Reservations",
                column: "PatientID",
                principalTable: "Patients",
                principalColumn: "PatientID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Patients_PatientID",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_PatientID",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "PatientID",
                table: "Reservations");

            migrationBuilder.AddColumn<int>(
                name: "VetID",
                table: "Reservations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_VetID",
                table: "Reservations",
                column: "VetID");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Users_VetID",
                table: "Reservations",
                column: "VetID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
