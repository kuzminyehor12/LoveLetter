using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class CardHistoryCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CardHistory",
                table: "States",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardHistory",
                table: "States");
        }
    }
}
