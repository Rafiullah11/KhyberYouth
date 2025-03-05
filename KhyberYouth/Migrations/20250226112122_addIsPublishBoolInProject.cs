using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KhyberYouth.Migrations
{
    /// <inheritdoc />
    public partial class addIsPublishBoolInProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPublished",
                table: "Projects",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublished",
                table: "Projects");
        }
    }
}
