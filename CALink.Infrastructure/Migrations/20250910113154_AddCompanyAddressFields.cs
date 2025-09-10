using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CALink.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyAddressFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "city",
                table: "Companies",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "country",
                table: "Companies",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "state",
                table: "Companies",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "zipcode",
                table: "Companies",
                type: "varchar(20)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "city",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "country",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "state",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "zipcode",
                table: "Companies");
        }
    }
}
