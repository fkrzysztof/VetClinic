using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VetClinic.Data.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PrescriptionItems",
                columns: table => new
                {
                    PrescriptionItemID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrescriptionID = table.Column<int>(nullable: true),
                    MedicineID = table.Column<int>(nullable: false),
                    Quantity = table.Column<decimal>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    AddedUserID = table.Column<int>(nullable: true),
                    UpdatedUserID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrescriptionItems", x => x.PrescriptionItemID);
                });

            migrationBuilder.CreateTable(
                name: "Medicines",
                columns: table => new
                {
                    MedicineID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicineTypeID = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    Quantity = table.Column<decimal>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    AddedUserID = table.Column<int>(nullable: true),
                    UpdatedUserID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicines", x => x.MedicineID);
                });

            migrationBuilder.CreateTable(
                name: "Visits",
                columns: table => new
                {
                    VisitID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VisitUserID = table.Column<int>(nullable: false),
                    PatientID = table.Column<int>(nullable: true),
                    VetID = table.Column<int>(nullable: true),
                    DateOfVisit = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    AddedUserID = table.Column<int>(nullable: true),
                    UpdatedUserID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visits", x => x.VisitID);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    PatientID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientTypeID = table.Column<int>(nullable: false),
                    PatientUserID = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    BirthDate = table.Column<DateTime>(nullable: false),
                    PatientNumber = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    AddedUserID = table.Column<int>(nullable: true),
                    UpdatedUserID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.PatientID);
                });

            migrationBuilder.CreateTable(
                name: "MedicineTypes",
                columns: table => new
                {
                    MedicineTypeID = table.Column<int>(nullable: false)
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
                    table.PrimaryKey("PK_MedicineTypes", x => x.MedicineTypeID);
                });

            migrationBuilder.CreateTable(
                name: "Operations",
                columns: table => new
                {
                    OperationID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VisitID = table.Column<int>(nullable: true),
                    VetID = table.Column<int>(nullable: true),
                    DateOfOperation = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    AddedUserID = table.Column<int>(nullable: true),
                    UpdatedUserID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operations", x => x.OperationID);
                    table.ForeignKey(
                        name: "FK_Operations_Visits_VisitID",
                        column: x => x.VisitID,
                        principalTable: "Visits",
                        principalColumn: "VisitID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PatientTypes",
                columns: table => new
                {
                    PatientTypeID = table.Column<int>(nullable: false)
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
                    table.PrimaryKey("PK_PatientTypes", x => x.PatientTypeID);
                });

            migrationBuilder.CreateTable(
                name: "Prescriptions",
                columns: table => new
                {
                    PrescriptionID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VisitID = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    TotalToPay = table.Column<decimal>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    AddedUserID = table.Column<int>(nullable: true),
                    UpdatedUserID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prescriptions", x => x.PrescriptionID);
                    table.ForeignKey(
                        name: "FK_Prescriptions_Visits_VisitID",
                        column: x => x.VisitID,
                        principalTable: "Visits",
                        principalColumn: "VisitID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecentNews",
                columns: table => new
                {
                    RecentNewsID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LinkTitle = table.Column<string>(maxLength: 20, nullable: false),
                    Title = table.Column<string>(maxLength: 50, nullable: false),
                    Text = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    Position = table.Column<int>(nullable: false),
                    Photo = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    AddedUserID = table.Column<int>(nullable: true),
                    UpdatedUserID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecentNews", x => x.RecentNewsID);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    ReservationID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReservationUserID = table.Column<int>(nullable: false),
                    VetID = table.Column<int>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    DateOfVisit = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    AddedUserID = table.Column<int>(nullable: true),
                    UpdatedUserID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.ReservationID);
                });

            migrationBuilder.CreateTable(
                name: "Statements",
                columns: table => new
                {
                    StatementID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderID = table.Column<int>(nullable: true),
                    ReceiverID = table.Column<int>(nullable: true),
                    Message = table.Column<string>(nullable: false),
                    IsReaded = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    AddedUserID = table.Column<int>(nullable: true),
                    UpdatedUserID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statements", x => x.StatementID);
                });

            migrationBuilder.CreateTable(
                name: "UserTypes",
                columns: table => new
                {
                    UserTypeID = table.Column<int>(nullable: false)
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
                    table.PrimaryKey("PK_UserTypes", x => x.UserTypeID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserTypeID = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: false),
                    PostalCode = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Login = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    Phone = table.Column<string>(nullable: true),
                    Photo = table.Column<string>(nullable: true),
                    CardNumber = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    LoginAttempt = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                    table.ForeignKey(
                        name: "FK_Users_UserTypes_UserTypeID",
                        column: x => x.UserTypeID,
                        principalTable: "UserTypes",
                        principalColumn: "UserTypeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Medicines_AddedUserID",
                table: "Medicines",
                column: "AddedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Medicines_MedicineTypeID",
                table: "Medicines",
                column: "MedicineTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Medicines_UpdatedUserID",
                table: "Medicines",
                column: "UpdatedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_MedicineTypes_AddedUserID",
                table: "MedicineTypes",
                column: "AddedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_MedicineTypes_UpdatedUserID",
                table: "MedicineTypes",
                column: "UpdatedUserID");

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
                name: "IX_Patients_AddedUserID",
                table: "Patients",
                column: "AddedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_PatientTypeID",
                table: "Patients",
                column: "PatientTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_PatientUserID",
                table: "Patients",
                column: "PatientUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_UpdatedUserID",
                table: "Patients",
                column: "UpdatedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientTypes_AddedUserID",
                table: "PatientTypes",
                column: "AddedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientTypes_UpdatedUserID",
                table: "PatientTypes",
                column: "UpdatedUserID");

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

            migrationBuilder.CreateIndex(
                name: "IX_RecentNews_AddedUserID",
                table: "RecentNews",
                column: "AddedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_RecentNews_UpdatedUserID",
                table: "RecentNews",
                column: "UpdatedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_AddedUserID",
                table: "Reservations",
                column: "AddedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_ReservationUserID",
                table: "Reservations",
                column: "ReservationUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_UpdatedUserID",
                table: "Reservations",
                column: "UpdatedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_VetID",
                table: "Reservations",
                column: "VetID");

            migrationBuilder.CreateIndex(
                name: "IX_Statements_AddedUserID",
                table: "Statements",
                column: "AddedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Statements_ReceiverID",
                table: "Statements",
                column: "ReceiverID");

            migrationBuilder.CreateIndex(
                name: "IX_Statements_SenderID",
                table: "Statements",
                column: "SenderID");

            migrationBuilder.CreateIndex(
                name: "IX_Statements_UpdatedUserID",
                table: "Statements",
                column: "UpdatedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserTypeID",
                table: "Users",
                column: "UserTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_UserTypes_AddedUserID",
                table: "UserTypes",
                column: "AddedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_UserTypes_UpdatedUserID",
                table: "UserTypes",
                column: "UpdatedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Visits_AddedUserID",
                table: "Visits",
                column: "AddedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Visits_PatientID",
                table: "Visits",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_Visits_UpdatedUserID",
                table: "Visits",
                column: "UpdatedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Visits_VetID",
                table: "Visits",
                column: "VetID");

            migrationBuilder.CreateIndex(
                name: "IX_Visits_VisitUserID",
                table: "Visits",
                column: "VisitUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_PrescriptionItems_Users_AddedUserID",
                table: "PrescriptionItems",
                column: "AddedUserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PrescriptionItems_Users_UpdatedUserID",
                table: "PrescriptionItems",
                column: "UpdatedUserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PrescriptionItems_Medicines_MedicineID",
                table: "PrescriptionItems",
                column: "MedicineID",
                principalTable: "Medicines",
                principalColumn: "MedicineID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PrescriptionItems_Prescriptions_PrescriptionID",
                table: "PrescriptionItems",
                column: "PrescriptionID",
                principalTable: "Prescriptions",
                principalColumn: "PrescriptionID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Medicines_Users_AddedUserID",
                table: "Medicines",
                column: "AddedUserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Medicines_Users_UpdatedUserID",
                table: "Medicines",
                column: "UpdatedUserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Medicines_MedicineTypes_MedicineTypeID",
                table: "Medicines",
                column: "MedicineTypeID",
                principalTable: "MedicineTypes",
                principalColumn: "MedicineTypeID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Visits_Users_AddedUserID",
                table: "Visits",
                column: "AddedUserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Visits_Users_UpdatedUserID",
                table: "Visits",
                column: "UpdatedUserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Visits_Users_VetID",
                table: "Visits",
                column: "VetID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Visits_Users_VisitUserID",
                table: "Visits",
                column: "VisitUserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Visits_Patients_PatientID",
                table: "Visits",
                column: "PatientID",
                principalTable: "Patients",
                principalColumn: "PatientID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Users_AddedUserID",
                table: "Patients",
                column: "AddedUserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Users_PatientUserID",
                table: "Patients",
                column: "PatientUserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Users_UpdatedUserID",
                table: "Patients",
                column: "UpdatedUserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_PatientTypes_PatientTypeID",
                table: "Patients",
                column: "PatientTypeID",
                principalTable: "PatientTypes",
                principalColumn: "PatientTypeID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicineTypes_Users_AddedUserID",
                table: "MedicineTypes",
                column: "AddedUserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicineTypes_Users_UpdatedUserID",
                table: "MedicineTypes",
                column: "UpdatedUserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Operations_Users_AddedUserID",
                table: "Operations",
                column: "AddedUserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Operations_Users_UpdatedUserID",
                table: "Operations",
                column: "UpdatedUserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Operations_Users_VetID",
                table: "Operations",
                column: "VetID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientTypes_Users_AddedUserID",
                table: "PatientTypes",
                column: "AddedUserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientTypes_Users_UpdatedUserID",
                table: "PatientTypes",
                column: "UpdatedUserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_Users_AddedUserID",
                table: "Prescriptions",
                column: "AddedUserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_Users_UpdatedUserID",
                table: "Prescriptions",
                column: "UpdatedUserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecentNews_Users_AddedUserID",
                table: "RecentNews",
                column: "AddedUserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecentNews_Users_UpdatedUserID",
                table: "RecentNews",
                column: "UpdatedUserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Users_AddedUserID",
                table: "Reservations",
                column: "AddedUserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Users_ReservationUserID",
                table: "Reservations",
                column: "ReservationUserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Users_UpdatedUserID",
                table: "Reservations",
                column: "UpdatedUserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Users_VetID",
                table: "Reservations",
                column: "VetID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Statements_Users_AddedUserID",
                table: "Statements",
                column: "AddedUserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Statements_Users_ReceiverID",
                table: "Statements",
                column: "ReceiverID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Statements_Users_SenderID",
                table: "Statements",
                column: "SenderID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Statements_Users_UpdatedUserID",
                table: "Statements",
                column: "UpdatedUserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTypes_Users_AddedUserID",
                table: "UserTypes",
                column: "AddedUserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTypes_Users_UpdatedUserID",
                table: "UserTypes",
                column: "UpdatedUserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTypes_Users_AddedUserID",
                table: "UserTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTypes_Users_UpdatedUserID",
                table: "UserTypes");

            migrationBuilder.DropTable(
                name: "Operations");

            migrationBuilder.DropTable(
                name: "PrescriptionItems");

            migrationBuilder.DropTable(
                name: "RecentNews");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "Statements");

            migrationBuilder.DropTable(
                name: "Medicines");

            migrationBuilder.DropTable(
                name: "Prescriptions");

            migrationBuilder.DropTable(
                name: "MedicineTypes");

            migrationBuilder.DropTable(
                name: "Visits");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "PatientTypes");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "UserTypes");
        }
    }
}
