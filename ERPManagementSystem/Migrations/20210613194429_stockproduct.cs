using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ERPManagementSystem.Migrations
{
    public partial class stockproduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StockProducts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Color = table.Column<string>(nullable: true),
                    Size = table.Column<string>(nullable: true),
                    SalePrice = table.Column<decimal>(nullable: false),
                    PerProductCost = table.Column<decimal>(nullable: false),
                    Discount = table.Column<decimal>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ImgPath = table.Column<string>(nullable: true),
                    StockProductStatus = table.Column<string>(nullable: true),
                    PurchaseOrderLineItemId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockProducts_PurchaseOrderLineItems_PurchaseOrderLineItemId",
                        column: x => x.PurchaseOrderLineItemId,
                        principalTable: "PurchaseOrderLineItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockProducts_ProductId",
                table: "StockProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StockProducts_PurchaseOrderLineItemId",
                table: "StockProducts",
                column: "PurchaseOrderLineItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockProducts");
        }
    }
}
