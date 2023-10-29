using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pesistence.Migrations
{
    /// <inheritdoc />
    public partial class addIsPermanent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "ServiceItem",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<bool>(
                name: "IsPermanent",
                table: "Member",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "B_IsPermanent",
                table: "Contract",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "ServiceItem");

            migrationBuilder.DropColumn(
                name: "IsPermanent",
                table: "Member");

            migrationBuilder.DropColumn(
                name: "B_IsPermanent",
                table: "Contract");
        }
    }
}
