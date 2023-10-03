using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pesistence.Migrations
{
    /// <inheritdoc />
    public partial class updateAcreage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Height",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "Length",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "Contract");

            migrationBuilder.DropColumn(
                name: "Length",
                table: "Contract");

            migrationBuilder.RenameColumn(
                name: "Width",
                table: "Room",
                newName: "Acreage");

            migrationBuilder.RenameColumn(
                name: "Width",
                table: "Contract",
                newName: "Acreage");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Acreage",
                table: "Room",
                newName: "Width");

            migrationBuilder.RenameColumn(
                name: "Acreage",
                table: "Contract",
                newName: "Width");

            migrationBuilder.AddColumn<float>(
                name: "Height",
                table: "Room",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Length",
                table: "Room",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Height",
                table: "Contract",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Length",
                table: "Contract",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
