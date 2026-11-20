using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CalendarManagement.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveExempel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Exempel");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Exempel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exempel", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Exempel",
                columns: new[] { "Id", "CreatedAt", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 11, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Första exemplet" },
                    { 2, new DateTime(2026, 11, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Andra exemplet" }
                });
        }
    }
}
