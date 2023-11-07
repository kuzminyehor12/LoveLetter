using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class LockedGameStateCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Locked",
                table: "States",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Locked",
                table: "States");
        }
    }
}
