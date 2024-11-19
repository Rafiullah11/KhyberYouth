using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KhyberYouth.Migrations
{
    /// <inheritdoc />
    public partial class updatedProjectEdit1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MainIcons",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MainIcons",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MainIcons",
                keyColumn: "Id",
                keyValue: 3);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "MainIcons",
                columns: new[] { "Id", "ActionName", "ControllerName", "Description", "Icons", "Title" },
                values: new object[,]
                {
                    { 1, "Index", "MediaGallery", "Explore our journey and see the impact we’re making together.", "fa-solid fa-music", "MEDIA" },
                    { 2, "Create", "Volunteers", "Join us to make a difference! Volunteer today and help bring positive change.", "fa-solid fa-bullhorn", "BECOME VOLUNTEER" },
                    { 3, "Privacy", "Home", "Your donation brings hope and change. Support our mission by donating now.", "fa-solid fa-hand-holding-dollar", "SEND DONATION" }
                });
        }
    }
}
