using Microsoft.EntityFrameworkCore.Migrations;

namespace ERPManagementSystem.Migrations
{
    public partial class addcitycountryandstatetableModefi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StateStatus",
                table: "States",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShippingChargeStatus",
                table: "ShippingCharges",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CountryStatus",
                table: "Countries",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CityStatus",
                table: "Cities",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StateStatus",
                table: "States");

            migrationBuilder.DropColumn(
                name: "ShippingChargeStatus",
                table: "ShippingCharges");

            migrationBuilder.DropColumn(
                name: "CountryStatus",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "CityStatus",
                table: "Cities");
        }
    }
}
