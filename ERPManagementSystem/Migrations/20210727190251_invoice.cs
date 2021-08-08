using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ERPManagementSystem.Migrations
{
    public partial class invoice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "InvoiceModelId",
                table: "PurchaseOrderLineItems",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "InvoiceModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PurchaseNo = table.Column<string>(nullable: true),
                    OrderDate = table.Column<DateTime>(nullable: false),
                    DeliveryDate = table.Column<DateTime>(nullable: false),
                    ShippingCost = table.Column<decimal>(nullable: false),
                    PurchaseOrderStatus = table.Column<string>(nullable: true),
                    Discont = table.Column<decimal>(nullable: false),
                    TotalAmount = table.Column<decimal>(nullable: false),
                    PurchaseOrderId = table.Column<Guid>(nullable: false),
                    VendorId = table.Column<Guid>(nullable: false),
                    CurrencyId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceModels_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvoiceModels_PurchaseOrders_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalTable: "PurchaseOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvoiceModels_Vendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceItemModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Color = table.Column<string>(nullable: true),
                    Size = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: true),
                    Discount = table.Column<decimal>(nullable: true),
                    Rate = table.Column<decimal>(nullable: true),
                    SalePrice = table.Column<decimal>(nullable: true),
                    PreviousPrice = table.Column<decimal>(nullable: true),
                    PerProductCost = table.Column<decimal>(nullable: true),
                    OrderQuantity = table.Column<int>(nullable: true),
                    ReceiveQuantity = table.Column<int>(nullable: true),
                    DueQuantity = table.Column<int>(nullable: true),
                    TotalCost = table.Column<decimal>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ShortDescription = table.Column<string>(nullable: true),
                    ImgPath = table.Column<string>(nullable: true),
                    ItemStatus = table.Column<string>(nullable: true),
                    ProductId = table.Column<Guid>(nullable: true),
                    TaxRateId = table.Column<Guid>(nullable: true),
                    InvoiceModelId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceItemModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceItemModels_InvoiceModels_InvoiceModelId",
                        column: x => x.InvoiceModelId,
                        principalTable: "InvoiceModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvoiceItemModels_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvoiceItemModels_TaxRates_TaxRateId",
                        column: x => x.TaxRateId,
                        principalTable: "TaxRates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderLineItems_InvoiceModelId",
                table: "PurchaseOrderLineItems",
                column: "InvoiceModelId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItemModels_InvoiceModelId",
                table: "InvoiceItemModels",
                column: "InvoiceModelId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItemModels_ProductId",
                table: "InvoiceItemModels",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItemModels_TaxRateId",
                table: "InvoiceItemModels",
                column: "TaxRateId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceModels_CurrencyId",
                table: "InvoiceModels",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceModels_PurchaseOrderId",
                table: "InvoiceModels",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceModels_VendorId",
                table: "InvoiceModels",
                column: "VendorId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderLineItems_InvoiceModels_InvoiceModelId",
                table: "PurchaseOrderLineItems",
                column: "InvoiceModelId",
                principalTable: "InvoiceModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrderLineItems_InvoiceModels_InvoiceModelId",
                table: "PurchaseOrderLineItems");

            migrationBuilder.DropTable(
                name: "InvoiceItemModels");

            migrationBuilder.DropTable(
                name: "InvoiceModels");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrderLineItems_InvoiceModelId",
                table: "PurchaseOrderLineItems");

            migrationBuilder.DropColumn(
                name: "InvoiceModelId",
                table: "PurchaseOrderLineItems");
        }
    }
}
