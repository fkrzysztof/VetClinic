﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VetClinic.Data;

namespace VetClinic.Data.Migrations
{
    [DbContext(typeof(VetClinicContext))]
    partial class VetClinicContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("VetClinic.Data.Data.CMS.RecentNews", b =>
                {
                    b.Property<int>("RecentNewsID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AddedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("AddedUserID")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("LinkTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("Photo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Position")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(MAX)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("UpdatedUserID")
                        .HasColumnType("int");

                    b.HasKey("RecentNewsID");

                    b.HasIndex("AddedUserID");

                    b.HasIndex("UpdatedUserID");

                    b.ToTable("RecentNews");
                });

            modelBuilder.Entity("VetClinic.Data.Data.Clinic.MedicalSpecialization", b =>
                {
                    b.Property<int>("MedicalSpecializationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AddedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("AddedUserID")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int>("SpecializationID")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("UpdatedUserID")
                        .HasColumnType("int");

                    b.Property<int?>("UserID")
                        .HasColumnType("int");

                    b.HasKey("MedicalSpecializationID");

                    b.HasIndex("AddedUserID");

                    b.HasIndex("SpecializationID");

                    b.HasIndex("UpdatedUserID");

                    b.HasIndex("UserID");

                    b.ToTable("MedicalSpecializations");
                });

            modelBuilder.Entity("VetClinic.Data.Data.Clinic.Medicine", b =>
                {
                    b.Property<int>("MedicineID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AddedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("AddedUserID")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int>("MedicineTypeID")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Quantity")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("UpdatedUserID")
                        .HasColumnType("int");

                    b.HasKey("MedicineID");

                    b.HasIndex("AddedUserID");

                    b.HasIndex("MedicineTypeID");

                    b.HasIndex("UpdatedUserID");

                    b.ToTable("Medicines");
                });

            modelBuilder.Entity("VetClinic.Data.Data.Clinic.MedicineType", b =>
                {
                    b.Property<int>("MedicineTypeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AddedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("AddedUserID")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("UpdatedUserID")
                        .HasColumnType("int");

                    b.HasKey("MedicineTypeID");

                    b.HasIndex("AddedUserID");

                    b.HasIndex("UpdatedUserID");

                    b.ToTable("MedicineTypes");
                });

            modelBuilder.Entity("VetClinic.Data.Data.Clinic.Operation", b =>
                {
                    b.Property<int>("OperationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AddedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("AddedUserID")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateOfOperation")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("UpdatedUserID")
                        .HasColumnType("int");

                    b.Property<int?>("VetID")
                        .HasColumnType("int");

                    b.Property<int?>("VisitID")
                        .HasColumnType("int");

                    b.HasKey("OperationID");

                    b.HasIndex("AddedUserID");

                    b.HasIndex("UpdatedUserID");

                    b.HasIndex("VetID");

                    b.HasIndex("VisitID");

                    b.ToTable("Operations");
                });

            modelBuilder.Entity("VetClinic.Data.Data.Clinic.Patient", b =>
                {
                    b.Property<int>("PatientID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AddedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("AddedUserID")
                        .HasColumnType("int");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PatientNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PatientTypeID")
                        .HasColumnType("int");

                    b.Property<int?>("PatientUserID")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("UpdatedUserID")
                        .HasColumnType("int");

                    b.HasKey("PatientID");

                    b.HasIndex("AddedUserID");

                    b.HasIndex("PatientTypeID");

                    b.HasIndex("PatientUserID");

                    b.HasIndex("UpdatedUserID");

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("VetClinic.Data.Data.Clinic.PatientType", b =>
                {
                    b.Property<int>("PatientTypeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AddedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("AddedUserID")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("UpdatedUserID")
                        .HasColumnType("int");

                    b.HasKey("PatientTypeID");

                    b.HasIndex("AddedUserID");

                    b.HasIndex("UpdatedUserID");

                    b.ToTable("PatientTypes");
                });

            modelBuilder.Entity("VetClinic.Data.Data.Clinic.Prescription", b =>
                {
                    b.Property<int>("PrescriptionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AddedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("AddedUserID")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<decimal>("TotalToPay")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("UpdatedUserID")
                        .HasColumnType("int");

                    b.Property<int>("VisitID")
                        .HasColumnType("int");

                    b.HasKey("PrescriptionID");

                    b.HasIndex("AddedUserID");

                    b.HasIndex("UpdatedUserID");

                    b.HasIndex("VisitID");

                    b.ToTable("Prescriptions");
                });

            modelBuilder.Entity("VetClinic.Data.Data.Clinic.PrescriptionItem", b =>
                {
                    b.Property<int>("PrescriptionItemID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AddedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("AddedUserID")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int>("MedicineID")
                        .HasColumnType("int");

                    b.Property<int?>("PrescriptionID")
                        .HasColumnType("int");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Quantity")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("UpdatedUserID")
                        .HasColumnType("int");

                    b.HasKey("PrescriptionItemID");

                    b.HasIndex("AddedUserID");

                    b.HasIndex("MedicineID");

                    b.HasIndex("PrescriptionID");

                    b.HasIndex("UpdatedUserID");

                    b.ToTable("PrescriptionItems");
                });

            modelBuilder.Entity("VetClinic.Data.Data.Clinic.Reservation", b =>
                {
                    b.Property<int>("ReservationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AddedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("AddedUserID")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateOfVisit")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int>("ReservationUserID")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("UpdatedUserID")
                        .HasColumnType("int");

                    b.Property<int?>("VetID")
                        .HasColumnType("int");

                    b.HasKey("ReservationID");

                    b.HasIndex("AddedUserID");

                    b.HasIndex("ReservationUserID");

                    b.HasIndex("UpdatedUserID");

                    b.HasIndex("VetID");

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("VetClinic.Data.Data.Clinic.Specialization", b =>
                {
                    b.Property<int>("SpecializationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AddedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("AddedUserID")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("UpdatedUserID")
                        .HasColumnType("int");

                    b.HasKey("SpecializationID");

                    b.HasIndex("AddedUserID");

                    b.HasIndex("UpdatedUserID");

                    b.ToTable("Specializations");
                });

            modelBuilder.Entity("VetClinic.Data.Data.Clinic.Statement", b =>
                {
                    b.Property<int>("StatementID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AddedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("AddedUserID")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsReaded")
                        .HasColumnType("bit");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ReceiverID")
                        .HasColumnType("int");

                    b.Property<int?>("SenderID")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("UpdatedUserID")
                        .HasColumnType("int");

                    b.HasKey("StatementID");

                    b.HasIndex("AddedUserID");

                    b.HasIndex("ReceiverID");

                    b.HasIndex("SenderID");

                    b.HasIndex("UpdatedUserID");

                    b.ToTable("Statements");
                });

            modelBuilder.Entity("VetClinic.Data.Data.Clinic.UserType", b =>
                {
                    b.Property<int>("UserTypeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AddedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("AddedUserID")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("UpdatedUserID")
                        .HasColumnType("int");

                    b.HasKey("UserTypeID");

                    b.HasIndex("AddedUserID");

                    b.HasIndex("UpdatedUserID");

                    b.ToTable("UserTypes");
                });

            modelBuilder.Entity("VetClinic.Data.Data.Clinic.Visit", b =>
                {
                    b.Property<int>("VisitID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AddedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("AddedUserID")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateOfVisit")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int?>("PatientID")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("UpdatedUserID")
                        .HasColumnType("int");

                    b.Property<int?>("VetID")
                        .HasColumnType("int");

                    b.Property<int>("VisitUserID")
                        .HasColumnType("int");

                    b.HasKey("VisitID");

                    b.HasIndex("AddedUserID");

                    b.HasIndex("PatientID");

                    b.HasIndex("UpdatedUserID");

                    b.HasIndex("VetID");

                    b.HasIndex("VisitUserID");

                    b.ToTable("Visits");
                });

            modelBuilder.Entity("VetClinic.Data.Data.VetClinic.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AddedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CardNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("LoginAttempt")
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Photo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserTypeID")
                        .HasColumnType("int");

                    b.HasKey("UserID");

                    b.HasIndex("UserTypeID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("VetClinic.Data.Data.CMS.RecentNews", b =>
                {
                    b.HasOne("VetClinic.Data.Data.VetClinic.User", "RecentNewsAddedUser")
                        .WithMany("AddedRecentNews")
                        .HasForeignKey("AddedUserID");

                    b.HasOne("VetClinic.Data.Data.VetClinic.User", "RecentNewsUpdatedUser")
                        .WithMany("UpdatedRecentNews")
                        .HasForeignKey("UpdatedUserID");
                });

            modelBuilder.Entity("VetClinic.Data.Data.Clinic.MedicalSpecialization", b =>
                {
                    b.HasOne("VetClinic.Data.Data.VetClinic.User", "MedicalSpecializationAddedUser")
                        .WithMany("AddedMedicalSpecialization")
                        .HasForeignKey("AddedUserID");

                    b.HasOne("VetClinic.Data.Data.Clinic.Specialization", "Specialization")
                        .WithMany("MedicalSpecializations")
                        .HasForeignKey("SpecializationID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VetClinic.Data.Data.VetClinic.User", "MedicalSpecializationUpdatedUser")
                        .WithMany("UpdatedMedicalSpecialization")
                        .HasForeignKey("UpdatedUserID");

                    b.HasOne("VetClinic.Data.Data.VetClinic.User", "MedicalSpecializationUser")
                        .WithMany("UserMedicalSpecialization")
                        .HasForeignKey("UserID");
                });

            modelBuilder.Entity("VetClinic.Data.Data.Clinic.Medicine", b =>
                {
                    b.HasOne("VetClinic.Data.Data.VetClinic.User", "MedicineAddedUser")
                        .WithMany("AddedMedicines")
                        .HasForeignKey("AddedUserID");

                    b.HasOne("VetClinic.Data.Data.Clinic.MedicineType", "MedicineType")
                        .WithMany("Medicines")
                        .HasForeignKey("MedicineTypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VetClinic.Data.Data.VetClinic.User", "MedicineUpdatedUser")
                        .WithMany("UpdatedMedicines")
                        .HasForeignKey("UpdatedUserID");
                });

            modelBuilder.Entity("VetClinic.Data.Data.Clinic.MedicineType", b =>
                {
                    b.HasOne("VetClinic.Data.Data.VetClinic.User", "MedicineTypeAddedUser")
                        .WithMany("AddedMedicineTypes")
                        .HasForeignKey("AddedUserID");

                    b.HasOne("VetClinic.Data.Data.VetClinic.User", "MedicineTypeUpdatedUser")
                        .WithMany("UpdatedMedicineTypes")
                        .HasForeignKey("UpdatedUserID");
                });

            modelBuilder.Entity("VetClinic.Data.Data.Clinic.Operation", b =>
                {
                    b.HasOne("VetClinic.Data.Data.VetClinic.User", "OperationAddedUser")
                        .WithMany("AddedOperations")
                        .HasForeignKey("AddedUserID");

                    b.HasOne("VetClinic.Data.Data.VetClinic.User", "OperationUpdatedUser")
                        .WithMany("UpdateOperations")
                        .HasForeignKey("UpdatedUserID");

                    b.HasOne("VetClinic.Data.Data.VetClinic.User", "VetUser")
                        .WithMany("VetOperations")
                        .HasForeignKey("VetID");

                    b.HasOne("VetClinic.Data.Data.Clinic.Visit", "Visit")
                        .WithMany("Operations")
                        .HasForeignKey("VisitID");
                });

            modelBuilder.Entity("VetClinic.Data.Data.Clinic.Patient", b =>
                {
                    b.HasOne("VetClinic.Data.Data.VetClinic.User", "PatientAddedUser")
                        .WithMany("AddedPatients")
                        .HasForeignKey("AddedUserID");

                    b.HasOne("VetClinic.Data.Data.Clinic.PatientType", "PatientType")
                        .WithMany("Patients")
                        .HasForeignKey("PatientTypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VetClinic.Data.Data.VetClinic.User", "PatientUser")
                        .WithMany("UserPatient")
                        .HasForeignKey("PatientUserID");

                    b.HasOne("VetClinic.Data.Data.VetClinic.User", "PatientUpdatedUser")
                        .WithMany("UpdatedPatients")
                        .HasForeignKey("UpdatedUserID");
                });

            modelBuilder.Entity("VetClinic.Data.Data.Clinic.PatientType", b =>
                {
                    b.HasOne("VetClinic.Data.Data.VetClinic.User", "PatientTypeAddedUser")
                        .WithMany("AddedPatientTypes")
                        .HasForeignKey("AddedUserID");

                    b.HasOne("VetClinic.Data.Data.VetClinic.User", "PatientTypeUpdatedUser")
                        .WithMany("UpdatedPatientTypes")
                        .HasForeignKey("UpdatedUserID");
                });

            modelBuilder.Entity("VetClinic.Data.Data.Clinic.Prescription", b =>
                {
                    b.HasOne("VetClinic.Data.Data.VetClinic.User", "PrescriptionAddedUser")
                        .WithMany("AddedPrescription")
                        .HasForeignKey("AddedUserID");

                    b.HasOne("VetClinic.Data.Data.VetClinic.User", "PrescriptionUpdatedUser")
                        .WithMany("UpdatedPrescription")
                        .HasForeignKey("UpdatedUserID");

                    b.HasOne("VetClinic.Data.Data.Clinic.Visit", "Visit")
                        .WithMany("Prescriptions")
                        .HasForeignKey("VisitID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("VetClinic.Data.Data.Clinic.PrescriptionItem", b =>
                {
                    b.HasOne("VetClinic.Data.Data.VetClinic.User", "PrescriptionItemAddedUser")
                        .WithMany("AddedPrescriptionItem")
                        .HasForeignKey("AddedUserID");

                    b.HasOne("VetClinic.Data.Data.Clinic.Medicine", "Medicine")
                        .WithMany("PrescriptionItems")
                        .HasForeignKey("MedicineID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VetClinic.Data.Data.Clinic.Prescription", "Prescription")
                        .WithMany("PrescriptionItems")
                        .HasForeignKey("PrescriptionID");

                    b.HasOne("VetClinic.Data.Data.VetClinic.User", "PrescriptionItemUpdatedUser")
                        .WithMany("UpdatedPrescriptionItem")
                        .HasForeignKey("UpdatedUserID");
                });

            modelBuilder.Entity("VetClinic.Data.Data.Clinic.Reservation", b =>
                {
                    b.HasOne("VetClinic.Data.Data.VetClinic.User", "ReservationAddedUser")
                        .WithMany("AddedReservation")
                        .HasForeignKey("AddedUserID");

                    b.HasOne("VetClinic.Data.Data.VetClinic.User", "ReservationUser")
                        .WithMany("ReservationUser")
                        .HasForeignKey("ReservationUserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VetClinic.Data.Data.VetClinic.User", "ReservationUpdatedUser")
                        .WithMany("UpdatedReservation")
                        .HasForeignKey("UpdatedUserID");

                    b.HasOne("VetClinic.Data.Data.VetClinic.User", "VetUser")
                        .WithMany("VetReservations")
                        .HasForeignKey("VetID");
                });

            modelBuilder.Entity("VetClinic.Data.Data.Clinic.Specialization", b =>
                {
                    b.HasOne("VetClinic.Data.Data.VetClinic.User", "SpecializationAddedUser")
                        .WithMany("AddedUserSpecialization")
                        .HasForeignKey("AddedUserID");

                    b.HasOne("VetClinic.Data.Data.VetClinic.User", "SpecializationUpdatedUser")
                        .WithMany("UpdatedUserSpecialization")
                        .HasForeignKey("UpdatedUserID");
                });

            modelBuilder.Entity("VetClinic.Data.Data.Clinic.Statement", b =>
                {
                    b.HasOne("VetClinic.Data.Data.VetClinic.User", "StatementAddedUser")
                        .WithMany("AddedStatements")
                        .HasForeignKey("AddedUserID");

                    b.HasOne("VetClinic.Data.Data.VetClinic.User", "ReceiverUser")
                        .WithMany("ReceiversStatement")
                        .HasForeignKey("ReceiverID");

                    b.HasOne("VetClinic.Data.Data.VetClinic.User", "SenderUser")
                        .WithMany("SendersStatement")
                        .HasForeignKey("SenderID");

                    b.HasOne("VetClinic.Data.Data.VetClinic.User", "StatementUpdatedUser")
                        .WithMany("UpdatedStatements")
                        .HasForeignKey("UpdatedUserID");
                });

            modelBuilder.Entity("VetClinic.Data.Data.Clinic.UserType", b =>
                {
                    b.HasOne("VetClinic.Data.Data.VetClinic.User", "UserTypeAddedUser")
                        .WithMany("AddedUserTypes")
                        .HasForeignKey("AddedUserID");

                    b.HasOne("VetClinic.Data.Data.VetClinic.User", "UserTypeUpdatedUser")
                        .WithMany("UpdatedUserTypes")
                        .HasForeignKey("UpdatedUserID");
                });

            modelBuilder.Entity("VetClinic.Data.Data.Clinic.Visit", b =>
                {
                    b.HasOne("VetClinic.Data.Data.VetClinic.User", "VisitAddedUser")
                        .WithMany("AddedVisits")
                        .HasForeignKey("AddedUserID");

                    b.HasOne("VetClinic.Data.Data.Clinic.Patient", "Patient")
                        .WithMany("Visits")
                        .HasForeignKey("PatientID");

                    b.HasOne("VetClinic.Data.Data.VetClinic.User", "VisitUpdatedUser")
                        .WithMany("UpdatedVisits")
                        .HasForeignKey("UpdatedUserID");

                    b.HasOne("VetClinic.Data.Data.VetClinic.User", "VetUser")
                        .WithMany("VetVisits")
                        .HasForeignKey("VetID");

                    b.HasOne("VetClinic.Data.Data.VetClinic.User", "VisitUser")
                        .WithMany("UserVisits")
                        .HasForeignKey("VisitUserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("VetClinic.Data.Data.VetClinic.User", b =>
                {
                    b.HasOne("VetClinic.Data.Data.Clinic.UserType", "UserType")
                        .WithMany("Users")
                        .HasForeignKey("UserTypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
