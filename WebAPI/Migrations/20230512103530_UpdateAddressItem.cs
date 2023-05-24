using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAddressItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AddressItems_AddressId",
                table: "AddressItems",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_AddressItems_AddressEntities_AddressId",
                table: "AddressItems",
                column: "AddressId",
                principalTable: "AddressEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AddressItems_AddressEntities_AddressId",
                table: "AddressItems");

            migrationBuilder.DropIndex(
                name: "IX_AddressItems_AddressId",
                table: "AddressItems");
        }
    }
}
