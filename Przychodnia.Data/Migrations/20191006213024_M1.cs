using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Przychodnia.Data.Migrations
{
    public partial class M1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Aktualnosci",
                columns: table => new
                {
                    IdAktualnosci = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LinkTytul = table.Column<string>(maxLength: 20, nullable: false),
                    Tytul = table.Column<string>(maxLength: 50, nullable: false),
                    Tekst = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    Pozycja = table.Column<int>(nullable: false),
                    Zdjecie = table.Column<byte[]>(nullable: true),
                    CzyAktywny = table.Column<bool>(nullable: false),
                    DataDodania = table.Column<DateTime>(nullable: false),
                    DataModyfikacji = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aktualnosci", x => x.IdAktualnosci);
                });

            migrationBuilder.CreateTable(
                name: "Uzytkownicy",
                columns: table => new
                {
                    IdUzytkownika = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Imie = table.Column<string>(nullable: false),
                    Nazwisko = table.Column<string>(nullable: false),
                    Ulica = table.Column<string>(nullable: true),
                    NrDomu = table.Column<string>(nullable: false),
                    Miejscowosc = table.Column<string>(nullable: false),
                    KodPocztowy = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Login = table.Column<string>(nullable: false),
                    Haslo = table.Column<string>(nullable: false),
                    Telefon = table.Column<string>(nullable: true),
                    Zdjecie = table.Column<string>(nullable: true),
                    NrKarty = table.Column<string>(nullable: true),
                    CzyAktywny = table.Column<bool>(nullable: false),
                    ProbaLogowania = table.Column<int>(nullable: false),
                    Opis = table.Column<string>(nullable: true),
                    DataDodania = table.Column<DateTime>(nullable: false),
                    DataModyfikacji = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uzytkownicy", x => x.IdUzytkownika);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Aktualnosci");

            migrationBuilder.DropTable(
                name: "Uzytkownicy");
        }
    }
}
