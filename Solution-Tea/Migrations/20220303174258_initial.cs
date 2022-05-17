using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solution_Tea.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Type",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Type", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tea",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Manufacturer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Debut = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tea", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tea_Type_TypeId",
                        column: x => x.TypeId,
                        principalTable: "Type",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Type",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "White" },
                    { 2, "Black" },
                    { 3, "Green" },
                    { 4, "Rooibos" },
                    { 5, "Herbal" }
                });

            migrationBuilder.InsertData(
                table: "Tea",
                columns: new[] { "Id", "Debut", "Manufacturer", "Name", "TypeId" },
                values: new object[] { 1, new DateTime(2010, 5, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Herbária", "Silver needles", 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Tea_TypeId",
                table: "Tea",
                column: "TypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tea");

            migrationBuilder.DropTable(
                name: "Type");
        }
    }
}
