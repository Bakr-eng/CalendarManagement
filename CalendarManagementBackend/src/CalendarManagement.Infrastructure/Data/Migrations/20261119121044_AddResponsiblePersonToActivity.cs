using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalendarManagement.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddResponsiblePersonToActivity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ResponsiblePersonId",
                table: "Activities",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ResponsiblePersonId",
                table: "Activities",
                column: "ResponsiblePersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Users_ResponsiblePersonId",
                table: "Activities",
                column: "ResponsiblePersonId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Users_ResponsiblePersonId",
                table: "Activities");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Activities_ResponsiblePersonId",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "ResponsiblePersonId",
                table: "Activities");
        }
    }
}
