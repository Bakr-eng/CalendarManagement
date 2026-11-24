using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalendarManagement.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerAndLocationToActivity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "Activities",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "Activities",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Activities_CustomerId",
                table: "Activities",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_Date",
                table: "Activities",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_LocationId",
                table: "Activities",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Customers_CustomerId",
                table: "Activities",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Locations_LocationId",
                table: "Activities",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Customers_CustomerId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Locations_LocationId",
                table: "Activities");

            migrationBuilder.DropIndex(
                name: "IX_Activities_CustomerId",
                table: "Activities");

            migrationBuilder.DropIndex(
                name: "IX_Activities_Date",
                table: "Activities");

            migrationBuilder.DropIndex(
                name: "IX_Activities_LocationId",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Activities");
        }
    }
}
