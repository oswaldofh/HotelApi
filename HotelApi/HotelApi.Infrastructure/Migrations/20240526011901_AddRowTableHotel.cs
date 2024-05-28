using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRowTableHotel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HotelTypeId",
                table: "Hotels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Hotels_HotelTypeId",
                table: "Hotels",
                column: "HotelTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Hotels_HotelTypes_HotelTypeId",
                table: "Hotels",
                column: "HotelTypeId",
                principalTable: "HotelTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hotels_HotelTypes_HotelTypeId",
                table: "Hotels");

            migrationBuilder.DropIndex(
                name: "IX_Hotels_HotelTypeId",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "HotelTypeId",
                table: "Hotels");
        }
    }
}
