using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JewelryAuctionApplicationDAL.Migrations
{
    /// <inheritdoc />
    public partial class init3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estimate",
                table: "Jewelry");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Estimate",
                table: "Jewelry",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
