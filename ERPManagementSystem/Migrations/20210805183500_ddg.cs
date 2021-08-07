using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ERPManagementSystem.Migrations
{
    public partial class ddg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SaleOrderItems_TaxRates_TaxRateId",
                table: "SaleOrderItems");

            migrationBuilder.DropIndex(
                name: "IX_SaleOrderItems_TaxRateId",
                table: "SaleOrderItems");

            migrationBuilder.DropColumn(
                name: "TaxRateId",
                table: "SaleOrderItems");

            migrationBuilder.AddColumn<decimal>(
                name: "TaxRate",
                table: "SaleOrderItems",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaxRate",
                table: "SaleOrderItems");

            migrationBuilder.AddColumn<Guid>(
                name: "TaxRateId",
                table: "SaleOrderItems",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_SaleOrderItems_TaxRateId",
                table: "SaleOrderItems",
                column: "TaxRateId");

            migrationBuilder.AddForeignKey(
                name: "FK_SaleOrderItems_TaxRates_TaxRateId",
                table: "SaleOrderItems",
                column: "TaxRateId",
                principalTable: "TaxRates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
