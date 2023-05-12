using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddressItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAddressItems_UserAddressItems_UserProfileAddressItemEntityUserProfileId_UserProfileAddressItemEntityAddressItemId",
                table: "UserAddressItems");

            migrationBuilder.DropIndex(
                name: "IX_UserAddressItems_UserProfileAddressItemEntityUserProfileId_UserProfileAddressItemEntityAddressItemId",
                table: "UserAddressItems");

            migrationBuilder.DropColumn(
                name: "UserProfileAddressItemEntityAddressItemId",
                table: "UserAddressItems");

            migrationBuilder.DropColumn(
                name: "UserProfileAddressItemEntityUserProfileId",
                table: "UserAddressItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserProfileAddressItemEntityAddressItemId",
                table: "UserAddressItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserProfileAddressItemEntityUserProfileId",
                table: "UserAddressItems",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAddressItems_UserProfileAddressItemEntityUserProfileId_UserProfileAddressItemEntityAddressItemId",
                table: "UserAddressItems",
                columns: new[] { "UserProfileAddressItemEntityUserProfileId", "UserProfileAddressItemEntityAddressItemId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserAddressItems_UserAddressItems_UserProfileAddressItemEntityUserProfileId_UserProfileAddressItemEntityAddressItemId",
                table: "UserAddressItems",
                columns: new[] { "UserProfileAddressItemEntityUserProfileId", "UserProfileAddressItemEntityAddressItemId" },
                principalTable: "UserAddressItems",
                principalColumns: new[] { "UserProfileId", "AddressItemId" });
        }
    }
}
