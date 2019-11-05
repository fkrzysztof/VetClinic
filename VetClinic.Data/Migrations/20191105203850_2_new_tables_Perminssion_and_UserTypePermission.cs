using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VetClinic.Data.Migrations
{
    public partial class _2_new_tables_Perminssion_and_UserTypePermission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    PermissionID = table.Column<int>(nullable: false)
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
                    table.PrimaryKey("PK_Permission", x => x.PermissionID);
                    table.ForeignKey(
                        name: "FK_Permission_Users_AddedUserID",
                        column: x => x.AddedUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Permission_Users_UpdatedUserID",
                        column: x => x.UpdatedUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserTypePermissions",
                columns: table => new
                {
                    UserPermissionID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserTypeID = table.Column<int>(nullable: false),
                    PermissionID = table.Column<int>(nullable: false),
                    Access = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    AddedUserID = table.Column<int>(nullable: true),
                    UpdatedUserID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTypePermissions", x => x.UserPermissionID);
                    table.ForeignKey(
                        name: "FK_UserTypePermissions_Users_AddedUserID",
                        column: x => x.AddedUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserTypePermissions_Permission_PermissionID",
                        column: x => x.PermissionID,
                        principalTable: "Permission",
                        principalColumn: "PermissionID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTypePermissions_Users_UpdatedUserID",
                        column: x => x.UpdatedUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserTypePermissions_UserTypes_UserTypeID",
                        column: x => x.UserTypeID,
                        principalTable: "UserTypes",
                        principalColumn: "UserTypeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Permission_AddedUserID",
                table: "Permission",
                column: "AddedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_UpdatedUserID",
                table: "Permission",
                column: "UpdatedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_UserTypePermissions_AddedUserID",
                table: "UserTypePermissions",
                column: "AddedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_UserTypePermissions_PermissionID",
                table: "UserTypePermissions",
                column: "PermissionID");

            migrationBuilder.CreateIndex(
                name: "IX_UserTypePermissions_UpdatedUserID",
                table: "UserTypePermissions",
                column: "UpdatedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_UserTypePermissions_UserTypeID",
                table: "UserTypePermissions",
                column: "UserTypeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserTypePermissions");

            migrationBuilder.DropTable(
                name: "Permission");
        }
    }
}
