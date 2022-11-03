using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MinimalAPI.Migrations
{
    public partial class PermissionsReference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UsersPermissionsId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsersPermissionsId",
                table: "Permissions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_UsersPermissionsId",
                table: "Users",
                column: "UsersPermissionsId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_UsersPermissionsId",
                table: "Permissions",
                column: "UsersPermissionsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_UsersPermissions_UsersPermissionsId",
                table: "Permissions",
                column: "UsersPermissionsId",
                principalTable: "UsersPermissions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UsersPermissions_UsersPermissionsId",
                table: "Users",
                column: "UsersPermissionsId",
                principalTable: "UsersPermissions",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_UsersPermissions_UsersPermissionsId",
                table: "Permissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_UsersPermissions_UsersPermissionsId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_UsersPermissionsId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Permissions_UsersPermissionsId",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "UsersPermissionsId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UsersPermissionsId",
                table: "Permissions");
        }
    }
}
