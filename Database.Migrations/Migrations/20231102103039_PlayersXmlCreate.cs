using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class PlayersXmlCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Players",
                table: "States",
                type: "Xml",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Players",
                table: "States",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "Xml",
                oldNullable: true);
        }
    }
}
