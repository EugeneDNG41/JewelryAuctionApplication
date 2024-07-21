using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JewelryAuctionApplicationDAL.Migrations
{
    /// <inheritdoc />
    public partial class NoMoreRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Request");

            migrationBuilder.AlterColumn<decimal>(
                name: "StartingPrice",
                table: "Jewelry",
                type: "decimal(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "BidAmount",
                table: "Bid",
                type: "decimal(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "CurrentPrice",
                table: "Auction",
                type: "decimal(18,0)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "Credit",
                table: "Account",
                type: "decimal(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "StartingPrice",
                table: "Jewelry",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "BidAmount",
                table: "Bid",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "CurrentPrice",
                table: "Auction",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,0)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "Credit",
                table: "Account",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,0)");

            migrationBuilder.CreateTable(
                name: "Request",
                columns: table => new
                {
                    RequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    JewelryId = table.Column<int>(type: "int", nullable: false),
                    FinalValuation = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PreliminaryValuation = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ValuationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Request", x => x.RequestId);
                    table.ForeignKey(
                        name: "FK_Request_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Request_Jewelry_JewelryId",
                        column: x => x.JewelryId,
                        principalTable: "Jewelry",
                        principalColumn: "JewelryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Request_AccountId",
                table: "Request",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Request_JewelryId",
                table: "Request",
                column: "JewelryId");
        }
    }
}
