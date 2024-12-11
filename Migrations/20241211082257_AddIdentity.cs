using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_Programlama_Proje_Odevi.Migrations
{
    /// <inheritdoc />
    public partial class AddIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SelectedServiceIds",
                table: "Employees");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SelectedServiceIds",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
