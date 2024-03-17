using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pesistence.Migrations
{
    /// <inheritdoc />
    public partial class updateContract2810 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GarbageColletionFee",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "InternetCosts",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "GarbageColletionFee",
                table: "Branch");

            migrationBuilder.DropColumn(
                name: "InternetCosts",
                table: "Branch");

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "Contract",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Contract");

            migrationBuilder.AddColumn<decimal>(
                name: "GarbageColletionFee",
                table: "Invoice",
                type: "decimal(10,0)",
                precision: 10,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "InternetCosts",
                table: "Invoice",
                type: "decimal(10,0)",
                precision: 10,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "GarbageColletionFee",
                table: "Branch",
                type: "decimal(10,0)",
                precision: 10,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "InternetCosts",
                table: "Branch",
                type: "decimal(10,0)",
                precision: 10,
                nullable: false,
                defaultValue: 0m);
        }
    }
}
