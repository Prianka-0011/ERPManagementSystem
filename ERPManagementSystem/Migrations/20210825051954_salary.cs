using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ERPManagementSystem.Migrations
{
    public partial class salary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeSalaryBills",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    SalaryBillStatus = table.Column<string>(nullable: true),
                    PaymentStatus = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeSalaryBills", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeSalaryBillItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SalaryAmount = table.Column<decimal>(nullable: false),
                    BonusAmount = table.Column<decimal>(nullable: false),
                    EmployeeId = table.Column<Guid>(nullable: false),
                    EmployeeSalaryBillId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeSalaryBillItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeSalaryBillItems_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeSalaryBillItems_EmployeeSalaryBills_EmployeeSalaryBi~",
                        column: x => x.EmployeeSalaryBillId,
                        principalTable: "EmployeeSalaryBills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSalaryBillItems_EmployeeId",
                table: "EmployeeSalaryBillItems",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSalaryBillItems_EmployeeSalaryBillId",
                table: "EmployeeSalaryBillItems",
                column: "EmployeeSalaryBillId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeSalaryBillItems");

            migrationBuilder.DropTable(
                name: "EmployeeSalaryBills");
        }
    }
}
