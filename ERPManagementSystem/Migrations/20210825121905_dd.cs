using Microsoft.EntityFrameworkCore.Migrations;

namespace ERPManagementSystem.Migrations
{
    public partial class dd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReferenceNo",
                table: "EmployeeSalaryBills",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalSalaryBill",
                table: "EmployeeSalaryBills",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReferenceNo",
                table: "EmployeeSalaryBills");

            migrationBuilder.DropColumn(
                name: "TotalSalaryBill",
                table: "EmployeeSalaryBills");
        }
    }
}
