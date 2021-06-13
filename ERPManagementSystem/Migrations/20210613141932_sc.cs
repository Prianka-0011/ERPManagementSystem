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

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderLineItems_QuotationId",
                table: "PurchaseOrderLineItems",
                column: "QuotationId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DesignationId",
                table: "Employees",
                column: "DesignationId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderLineItems_Products_ProductId",
                table: "PurchaseOrderLineItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderLineItems_PurchaseOrders_PurchaseOrderId",
                table: "PurchaseOrderLineItems",
                column: "PurchaseOrderId",
                principalTable: "PurchaseOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderLineItems_Quotations_QuotationId",
                table: "PurchaseOrderLineItems",
                column: "QuotationId",
                principalTable: "Quotations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderLineItems_TaxRates_TaxRateId",
                table: "PurchaseOrderLineItems",
                column: "TaxRateId",
                principalTable: "TaxRates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrderLineItems_Products_ProductId",
                table: "PurchaseOrderLineItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrderLineItems_PurchaseOrders_PurchaseOrderId",
                table: "PurchaseOrderLineItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrderLineItems_Quotations_QuotationId",
                table: "PurchaseOrderLineItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrderLineItems_TaxRates_TaxRateId",
                table: "PurchaseOrderLineItems");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrderLineItems_QuotationId",
                table: "PurchaseOrderLineItems");

            migrationBuilder.DropColumn(
                name: "QuotationId",
                table: "PurchaseOrderLineItems");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "TaxRates",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalCost",
                table: "PurchaseOrderLineItems",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "TaxRateId",
                table: "PurchaseOrderLineItems",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ReceiveQuantity",
                table: "PurchaseOrderLineItems",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Rate",
                table: "PurchaseOrderLineItems",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "PurchaseOrderId",
                table: "PurchaseOrderLineItems",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "PurchaseOrderLineItems",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "PurchaseOrderLineItems",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "PerProductCost",
                table: "PurchaseOrderLineItems",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OrderQuantity",
                table: "PurchaseOrderLineItems",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DueQuantity",
                table: "PurchaseOrderLineItems",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Discount",
                table: "PurchaseOrderLineItems",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderLineItems_Products_ProductId",
                table: "PurchaseOrderLineItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderLineItems_PurchaseOrders_PurchaseOrderId",
                table: "PurchaseOrderLineItems",
                column: "PurchaseOrderId",
                principalTable: "PurchaseOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderLineItems_TaxRates_TaxRateId",
                table: "PurchaseOrderLineItems",
                column: "TaxRateId",
                principalTable: "TaxRates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
