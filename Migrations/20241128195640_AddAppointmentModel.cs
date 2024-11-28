using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_Programlama_Proje_Odevi.Migrations
{
    /// <inheritdoc />
    public partial class AddAppointmentModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Appointments",
                newName: "AppointmentDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AppointmentDate",
                table: "Appointments",
                newName: "Date");
        }
    }
}
