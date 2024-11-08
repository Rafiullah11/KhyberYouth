using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KhyberYouth.Migrations
{
    /// <inheritdoc />
    public partial class AddDeptEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Dept",
                table: "TeamMembers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dept",
                table: "TeamMembers");
        }
    }
}
