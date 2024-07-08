using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JewelryAuctionApplicationDAL.Migrations
{
    /// <inheritdoc />
    public partial class noAuctionStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Auction");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Auction",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
