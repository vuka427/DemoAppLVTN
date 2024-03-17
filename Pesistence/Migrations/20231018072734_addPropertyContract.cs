using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pesistence.Migrations
{
    /// <inheritdoc />
    public partial class addPropertyContract : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ElectricityCosts",
                table: "Contract",
                type: "decimal(10,0)",
                precision: 10,
                scale: 0,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "WaterCosts",
                table: "Contract",
                type: "decimal(10,0)",
                precision: 10,
                scale: 0,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ElectricityCosts",
                table: "Contract");

            migrationBuilder.DropColumn(
                name: "WaterCosts",
                table: "Contract");
        }
    }
}
