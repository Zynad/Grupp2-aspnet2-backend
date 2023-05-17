using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class Creditcard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CreditCardEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameOnCard = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CardNo = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Expires = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CVV = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    UserProfileId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditCardEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreditCardEntities_UserProfileEntities_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfileEntities",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CreditCardEntities_UserProfileId",
                table: "CreditCardEntities",
                column: "UserProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CreditCardEntities");
        }
    }
}
