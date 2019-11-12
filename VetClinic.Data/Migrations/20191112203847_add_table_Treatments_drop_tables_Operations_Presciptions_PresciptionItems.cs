using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VetClinic.Data.Migrations
{
    public partial class add_table_Treatments_drop_tables_Operations_Presciptions_PresciptionItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Operations");

            migrationBuilder.DropTable(
                name: "PrescriptionItems");

            migrationBuilder.DropTable(
                name: "Prescriptions");

            migrationBuilder.AddColumn<int>(
                name: "TreatmentID",
                table: "Visits",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Treatments",
                columns: table => new
                {
                    TreatmentID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    AddedUserID = table.Column<int>(nullable: true),
                    UpdatedUserID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Treatments", x => x.TreatmentID);
                    table.ForeignKey(
                        name: "FK_Treatments_Users_AddedUserID",
                        column: x => x.AddedUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Treatments_Users_UpdatedUserID",
                        column: x => x.UpdatedUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Visits_TreatmentID",
                table: "Visits",
                column: "TreatmentID");

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_AddedUserID",
                table: "Treatments",
                column: "AddedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_UpdatedUserID",
                table: "Treatments",
                column: "UpdatedUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Visits_Treatments_TreatmentID",
                table: "Visits",
                column: "TreatmentID",
                principalTable: "Treatments",
                principalColumn: "TreatmentID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Visits_Treatments_TreatmentID",
                table: "Visits");

            migrationBuilder.DropTable(
                name: "Treatments");

            migrationBuilder.DropIndex(
                name: "IX_Visits_TreatmentID",
                table: "Visits");

            migrationBuilder.DropColumn(
                name: "TreatmentID",
                table: "Visits");

            migrationBuilder.CreateTable(
                name: "Operations",
                columns: table => new
                {
                    OperationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AddedUserID = table.Column<int>(type: "int", nullable: true),
                    DateOfOperation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedUserID = table.Column<int>(type: "int", nullable: true),
                    VetID = table.Column<int>(type: "int", nullable: true),
                    VisitID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operations", x => x.OperationID);
                    table.ForeignKey(
                        name: "FK_Operations_Users_AddedUserID",
                        column: x => x.AddedUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Operations_Users_UpdatedUserID",
                        column: x => x.UpdatedUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Operations_Users_VetID",
                        column: x => x.VetID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Operations_Visits_VisitID",
                        column: x => x.VisitID,
                        principalTable: "Visits",
                        principalColumn: "VisitID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Prescriptions",
                columns: table => new
                {
                    PrescriptionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AddedUserID = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    TotalToPay = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedUserID = table.Column<int>(type: "int", nullable: true),
                    VisitID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prescriptions", x => x.PrescriptionID);
                    table.ForeignKey(
                        name: "FK_Prescriptions_Users_AddedUserID",
                        column: x => x.AddedUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Prescriptions_Users_UpdatedUserID",
                        column: x => x.UpdatedUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Prescriptions_Visits_VisitID",
                        column: x => x.VisitID,
                        principalTable: "Visits",
                        principalColumn: "VisitID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrescriptionItems",
                columns: table => new
                {
                    PrescriptionItemID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AddedUserID = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    MedicineID = table.Column<int>(type: "int", nullable: false),
                    PrescriptionID = table.Column<int>(type: "int", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedUserID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrescriptionItems", x => x.PrescriptionItemID);
                    table.ForeignKey(
                        name: "FK_PrescriptionItems_Users_AddedUserID",
                        column: x => x.AddedUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PrescriptionItems_Medicines_MedicineID",
                        column: x => x.MedicineID,
                        principalTable: "Medicines",
                        principalColumn: "MedicineID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrescriptionItems_Prescriptions_PrescriptionID",
                        column: x => x.PrescriptionID,
                        principalTable: "Prescriptions",
                        principalColumn: "PrescriptionID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PrescriptionItems_Users_UpdatedUserID",
                        column: x => x.UpdatedUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Operations_AddedUserID",
                table: "Operations",
                column: "AddedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_UpdatedUserID",
                table: "Operations",
                column: "UpdatedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_VetID",
                table: "Operations",
                column: "VetID");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_VisitID",
                table: "Operations",
                column: "VisitID");

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionItems_AddedUserID",
                table: "PrescriptionItems",
                column: "AddedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionItems_MedicineID",
                table: "PrescriptionItems",
                column: "MedicineID");

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionItems_PrescriptionID",
                table: "PrescriptionItems",
                column: "PrescriptionID");

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionItems_UpdatedUserID",
                table: "PrescriptionItems",
                column: "UpdatedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_AddedUserID",
                table: "Prescriptions",
                column: "AddedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_UpdatedUserID",
                table: "Prescriptions",
                column: "UpdatedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_VisitID",
                table: "Prescriptions",
                column: "VisitID");
        }
    }
}
