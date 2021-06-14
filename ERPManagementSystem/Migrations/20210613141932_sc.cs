using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ERPManagementSystem.Migrations
{
    public partial class sc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    EmployeeType = table.Column<string>(nullable: true),
                    Salary = table.Column<decimal>(nullable: false),
                    ReviewSalary = table.Column<decimal>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false),
                    DesignationId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Designations_DesignationId",
                        column: x => x.DesignationId,
                        principalTable: "Designations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

    }
}
