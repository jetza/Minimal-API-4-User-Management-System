using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MinimalAPI.Migrations
{
    public partial class ModifyPermissionProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_UsersPermissions_UsersPermissionsId",
                table: "Permissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_UsersPermissions_UsersPermissionsId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "UsersPermissions");

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

            migrationBuilder.CreateTable(
                name: "UserPermissions",
                columns: table => new
                {
                    PermissionsId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPermissions", x => new { x.PermissionsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_UserPermissions_Permissions_PermissionsId",
                        column: x => x.PermissionsId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPermissions_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissions_UsersId",
                table: "UserPermissions",
                column: "UsersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserPermissions");

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

            migrationBuilder.CreateTable(
                name: "UsersPermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PermissionId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersPermissions", x => x.Id);
                });

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
    }
}
