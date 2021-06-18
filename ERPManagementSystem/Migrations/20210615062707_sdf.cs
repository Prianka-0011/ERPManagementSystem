using Microsoft.EntityFrameworkCore.Migrations;

namespace ERPManagementSystem.Migrations
{
    public partial class sdf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShortDescription",
                table: "PurchaseOrderLineItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeStatus",
                table: "Employees",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShortDescription",
                table: "PurchaseOrderLineItems");

            migrationBuilder.DropColumn(
                name: "EmployeeStatus",
                table: "Employees");
        }
    }
}
