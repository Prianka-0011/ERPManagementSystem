using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ERPManagementSystem.Migrations
{
    public partial class cahsTrnasition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cashes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TransitionNo = table.Column<string>(nullable: true),
                    LastTransitionAmout = table.Column<decimal>(nullable: false),
                    TotalBalance = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cashes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransitionBlances",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    TransitionNo = table.Column<string>(nullable: true),
                    TransBalance = table.Column<decimal>(nullable: false),
                    TransitionType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransitionBlances", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cashes");

            migrationBuilder.DropTable(
                name: "TransitionBlances");
        }
    }
}
