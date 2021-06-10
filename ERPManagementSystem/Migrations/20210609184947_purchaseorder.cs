using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ERPManagementSystem.Migrations
{
    public partial class purchaseorder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PurchaseOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PurchaseNo = table.Column<string>(nullable: true),
                    OrderDate = table.Column<DateTime>(nullable: false),
                    DeliveryDate = table.Column<DateTime>(nullable: false),
                    ShippingCost = table.Column<decimal>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    Discont = table.Column<decimal>(nullable: false),
                    VendorId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_Vendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrderLineItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Color = table.Column<string>(nullable: true),
                    Size = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    Discount = table.Column<decimal>(nullable: false),
                    Rate = table.Column<decimal>(nullable: false),
                    PerProductCost = table.Column<decimal>(nullable: false),
                    OrderQuantity = table.Column<int>(nullable: false),
                    ReceiveQuantity = table.Column<int>(nullable: false),
                    DueQuantity = table.Column<int>(nullable: false),
                    TotalCost = table.Column<decimal>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ImgPath = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    PurchaseOrderId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<Guid>(nullable: false),
                    QuotationId = table.Column<Guid>(nullable: false),
                    TaxRateId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrderLineItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderLineItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderLineItems_PurchaseOrders_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalTable: "PurchaseOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderLineItems_Quotations_QuotationId",
                        column: x => x.QuotationId,
                        principalTable: "Quotations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderLineItems_TaxRates_TaxRateId",
                        column: x => x.TaxRateId,
                        principalTable: "TaxRates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderLineItems_ProductId",
                table: "PurchaseOrderLineItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderLineItems_PurchaseOrderId",
                table: "PurchaseOrderLineItems",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderLineItems_QuotationId",
                table: "PurchaseOrderLineItems",
                column: "QuotationId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderLineItems_TaxRateId",
                table: "PurchaseOrderLineItems",
                column: "TaxRateId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_VendorId",
                table: "PurchaseOrders",
                column: "VendorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchaseOrderLineItems");

            migrationBuilder.DropTable(
                name: "PurchaseOrders");
        }
    }
}
