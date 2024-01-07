using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class FixAdDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LastViewedHistories_Ads_AdId",
                table: "LastViewedHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Ads_AdId",
                table: "Purchases");

            migrationBuilder.DropForeignKey(
                name: "FK_UserReviews_Ads_AdId",
                table: "UserReviews");

            migrationBuilder.AddForeignKey(
                name: "FK_LastViewedHistories_Ads_AdId",
                table: "LastViewedHistories",
                column: "AdId",
                principalTable: "Ads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Ads_AdId",
                table: "Purchases",
                column: "AdId",
                principalTable: "Ads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserReviews_Ads_AdId",
                table: "UserReviews",
                column: "AdId",
                principalTable: "Ads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LastViewedHistories_Ads_AdId",
                table: "LastViewedHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Ads_AdId",
                table: "Purchases");

            migrationBuilder.DropForeignKey(
                name: "FK_UserReviews_Ads_AdId",
                table: "UserReviews");

            migrationBuilder.AddForeignKey(
                name: "FK_LastViewedHistories_Ads_AdId",
                table: "LastViewedHistories",
                column: "AdId",
                principalTable: "Ads",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Ads_AdId",
                table: "Purchases",
                column: "AdId",
                principalTable: "Ads",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserReviews_Ads_AdId",
                table: "UserReviews",
                column: "AdId",
                principalTable: "Ads",
                principalColumn: "Id");
        }
    }
}
