using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ERPManagementSystem.Migrations
{
    public partial class scripter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaxRates",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Rate = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxRates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vendor",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FristName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    CompanyName = table.Column<string>(nullable: false),
                    DisplayName = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: false),
                    WebSite = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Quotations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    QuotationNo = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    ShippingCost = table.Column<decimal>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    VendorId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quotations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Quotations_Vendor_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuotationLineItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Color = table.Column<string>(nullable: true),
                    Size = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    Discount = table.Column<int>(nullable: false),
                    PerProductCost = table.Column<decimal>(nullable: false),
                    Quantity = table.Column<decimal>(nullable: false),
                    TotalCost = table.Column<decimal>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ProductId = table.Column<Guid>(nullable: false),
                    QuotationId = table.Column<Guid>(nullable: false),
                    TaxRateId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuotationLineItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuotationLineItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuotationLineItems_Quotations_QuotationId",
                        column: x => x.QuotationId,
                        principalTable: "Quotations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuotationLineItems_TaxRates_TaxRateId",
                        column: x => x.TaxRateId,
                        principalTable: "TaxRates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuotationLineItems_ProductId",
                table: "QuotationLineItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationLineItems_QuotationId",
                table: "QuotationLineItems",
                column: "QuotationId");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationLineItems_TaxRateId",
                table: "QuotationLineItems",
                column: "TaxRateId");

            migrationBuilder.CreateIndex(
                name: "IX_Quotations_VendorId",
                table: "Quotations",
                column: "VendorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuotationLineItems");

            migrationBuilder.DropTable(
                name: "Quotations");

            migrationBuilder.DropTable(
                name: "TaxRates");

            migrationBuilder.DropTable(
                name: "Vendor");
        }
    }
}
