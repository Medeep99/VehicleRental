using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleRentalProject.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddRepository : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MyProperty",
                table: "Rental",
                newName: "RentalStatus");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RentalStatus",
                table: "Rental",
                newName: "MyProperty");
        }
    }
}
