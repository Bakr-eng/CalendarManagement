using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalendarManagement.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerTypeAndIdentityNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerType",
                table: "Customers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "IdentityNumber",
                table: "Customers",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerType",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "IdentityNumber",
                table: "Customers");
        }
    }
}
