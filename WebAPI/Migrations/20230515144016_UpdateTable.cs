using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CreditCardEntities_UserProfileEntities_UserProfileId",
                table: "CreditCardEntities");

            migrationBuilder.DropIndex(
                name: "IX_CreditCardEntities_UserProfileId",
                table: "CreditCardEntities");

            migrationBuilder.DropColumn(
                name: "UserProfileId",
                table: "CreditCardEntities");

            migrationBuilder.CreateTable(
                name: "UserProfileCreditCards",
                columns: table => new
                {
                    UserProfileId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreditCardId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfileCreditCards", x => new { x.UserProfileId, x.CreditCardId });
                    table.ForeignKey(
                        name: "FK_UserProfileCreditCards_CreditCardEntities_CreditCardId",
                        column: x => x.CreditCardId,
                        principalTable: "CreditCardEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserProfileCreditCards_UserProfileEntities_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfileEntities",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserProfileCreditCards_CreditCardId",
                table: "UserProfileCreditCards",
                column: "CreditCardId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserProfileCreditCards");

            migrationBuilder.AddColumn<string>(
                name: "UserProfileId",
                table: "CreditCardEntities",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_CreditCardEntities_UserProfileId",
                table: "CreditCardEntities",
                column: "UserProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_CreditCardEntities_UserProfileEntities_UserProfileId",
                table: "CreditCardEntities",
                column: "UserProfileId",
                principalTable: "UserProfileEntities",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
