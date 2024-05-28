using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RoomStatus",
                table: "Bookings",
                newName: "BookingStatus");

            migrationBuilder.AddColumn<decimal>(
                name: "ValueIva",
                table: "Bookings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValueIva",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "BookingStatus",
                table: "Bookings",
                newName: "RoomStatus");
        }
    }
}
