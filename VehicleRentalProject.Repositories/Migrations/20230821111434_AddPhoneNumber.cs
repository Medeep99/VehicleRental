using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleRentalProject.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddPhoneNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PohoneNumber",
                table: "UserDetails",
                newName: "PhoneNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "UserDetails",
                newName: "PohoneNumber");
        }
    }
}
