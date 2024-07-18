using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleRentalProject.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Rental");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Rental");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Rental");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Rental");

            migrationBuilder.AddColumn<string>(
                name: "PaymentIntentId",
                table: "Rental",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SessionId",
                table: "Rental",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentIntentId",
                table: "Rental");

            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "Rental");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Rental",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Rental",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Rental",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Rental",
                type: "datetime2",
                nullable: true);
        }
    }
}
