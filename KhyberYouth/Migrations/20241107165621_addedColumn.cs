using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KhyberYouth.Migrations
{
    /// <inheritdoc />
    public partial class addedColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Contacts",
                newName: "Subject");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Subject",
                table: "Contacts",
                newName: "PhoneNumber");
        }
    }
}
