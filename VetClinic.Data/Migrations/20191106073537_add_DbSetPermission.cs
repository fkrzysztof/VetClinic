using Microsoft.EntityFrameworkCore.Migrations;

namespace VetClinic.Data.Migrations
{
    public partial class add_DbSetPermission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permission_Users_AddedUserID",
                table: "Permission");

            migrationBuilder.DropForeignKey(
                name: "FK_Permission_Users_UpdatedUserID",
                table: "Permission");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTypePermissions_Permission_PermissionID",
                table: "UserTypePermissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Permission",
                table: "Permission");

            migrationBuilder.RenameTable(
                name: "Permission",
                newName: "Permissions");

            migrationBuilder.RenameIndex(
                name: "IX_Permission_UpdatedUserID",
                table: "Permissions",
                newName: "IX_Permissions_UpdatedUserID");

            migrationBuilder.RenameIndex(
                name: "IX_Permission_AddedUserID",
                table: "Permissions",
                newName: "IX_Permissions_AddedUserID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Permissions",
                table: "Permissions",
                column: "PermissionID");

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_Users_AddedUserID",
                table: "Permissions",
                column: "AddedUserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_Users_UpdatedUserID",
                table: "Permissions",
                column: "UpdatedUserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTypePermissions_Permissions_PermissionID",
                table: "UserTypePermissions",
                column: "PermissionID",
                principalTable: "Permissions",
                principalColumn: "PermissionID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_Users_AddedUserID",
                table: "Permissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_Users_UpdatedUserID",
                table: "Permissions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTypePermissions_Permissions_PermissionID",
                table: "UserTypePermissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Permissions",
                table: "Permissions");

            migrationBuilder.RenameTable(
                name: "Permissions",
                newName: "Permission");

            migrationBuilder.RenameIndex(
                name: "IX_Permissions_UpdatedUserID",
                table: "Permission",
                newName: "IX_Permission_UpdatedUserID");

            migrationBuilder.RenameIndex(
                name: "IX_Permissions_AddedUserID",
                table: "Permission",
                newName: "IX_Permission_AddedUserID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Permission",
                table: "Permission",
                column: "PermissionID");

            migrationBuilder.AddForeignKey(
                name: "FK_Permission_Users_AddedUserID",
                table: "Permission",
                column: "AddedUserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Permission_Users_UpdatedUserID",
                table: "Permission",
                column: "UpdatedUserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTypePermissions_Permission_PermissionID",
                table: "UserTypePermissions",
                column: "PermissionID",
                principalTable: "Permission",
                principalColumn: "PermissionID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
