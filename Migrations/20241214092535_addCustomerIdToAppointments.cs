using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_Programlama_Proje_Odevi.Migrations
{
    /// <inheritdoc />
    public partial class addCustomerIdToAppointments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Appointments");
        }
    }
}
