using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class AddUserInformation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "UserInfoId",
                table: "AspNetUsers",
                type: "decimal(20,0)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserConnections",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DateLogin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateLogout = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserDevice = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserConnections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserConnections_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserInfoId",
                table: "AspNetUsers",
                column: "UserInfoId",
                unique: true,
                filter: "[UserInfoId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserConnections_UserId",
                table: "UserConnections",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_UserConnections_UserInfoId",
                table: "AspNetUsers",
                column: "UserInfoId",
                principalTable: "UserConnections",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_UserConnections_UserInfoId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "UserConnections");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UserInfoId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserInfoId",
                table: "AspNetUsers");
        }
    }
}
