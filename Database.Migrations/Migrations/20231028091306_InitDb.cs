using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class InitDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lobbies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<short>(type: "smallint", nullable: false),
                    Players = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lobbies", x => x.Id);
                    table.CheckConstraint("CK_Lobbies_Status_Enum", "[Status] IN (CAST(0 AS smallint), CAST(1 AS smallint), CAST(2 AS smallint))");
                });

            migrationBuilder.CreateTable(
                name: "States",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Players = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Deck = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TurnPlayerNumber = table.Column<short>(type: "smallint", nullable: false),
                    WinnerPlayerNumber = table.Column<short>(type: "smallint", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_States", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LobbyState",
                        column: x => x.Id,
                        principalTable: "Lobbies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Audit",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GameStateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlayerNickname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlayerNumber = table.Column<short>(type: "smallint", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Audit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StateAuditItems",
                        column: x => x.GameStateId,
                        principalTable: "States",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Audit_GameStateId",
                table: "Audit",
                column: "GameStateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Audit");

            migrationBuilder.DropTable(
                name: "States");

            migrationBuilder.DropTable(
                name: "Lobbies");
        }
    }
}
