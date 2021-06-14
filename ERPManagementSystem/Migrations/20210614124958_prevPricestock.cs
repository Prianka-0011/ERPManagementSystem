using Microsoft.EntityFrameworkCore.Migrations;

namespace ERPManagementSystem.Migrations
{
    public partial class prevPricestock : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PreviousPrice",
                table: "StockProducts",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreviousPrice",
                table: "StockProducts");
        }
    }
}
