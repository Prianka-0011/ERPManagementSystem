using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ERPManagementSystem.Migrations
{
    public partial class currencyAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrencyName",
                table: "StockProducts",
                nullable: true);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           


            migrationBuilder.DropColumn(
                name: "CurrencyName",
                table: "StockProducts");

       

        }
    }
}
