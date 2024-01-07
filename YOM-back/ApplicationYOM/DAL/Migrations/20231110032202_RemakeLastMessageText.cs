using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class RemakeLastMessageText : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastMessageText",
                table: "UserConversations");

            migrationBuilder.DropColumn(
                name: "LastConversationConnection",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "LastMessageText",
                table: "Conversations",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastMessageText",
                table: "Conversations");

            migrationBuilder.AddColumn<string>(
                name: "LastMessageText",
                table: "UserConversations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastConversationConnection",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
