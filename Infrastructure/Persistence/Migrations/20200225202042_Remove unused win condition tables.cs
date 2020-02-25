using Microsoft.EntityFrameworkCore.Migrations;

namespace TicTacToe.Infrastructure.Persistence.Migrations
{
    public partial class Removeunusedwinconditiontables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CrossPlayerGameWinConditions");

            migrationBuilder.DropTable(
                name: "NoughtPlayerGameWinConditions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CrossPlayerGameWinConditions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    WinConditionId = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrossPlayerGameWinConditions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrossPlayerGameWinConditions_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CrossPlayerGameWinConditions_WinConditions_WinConditionId",
                        column: x => x.WinConditionId,
                        principalTable: "WinConditions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NoughtPlayerGameWinConditions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    WinConditionId = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoughtPlayerGameWinConditions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NoughtPlayerGameWinConditions_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NoughtPlayerGameWinConditions_WinConditions_WinConditionId",
                        column: x => x.WinConditionId,
                        principalTable: "WinConditions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CrossPlayerGameWinConditions_GameId",
                table: "CrossPlayerGameWinConditions",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_CrossPlayerGameWinConditions_WinConditionId",
                table: "CrossPlayerGameWinConditions",
                column: "WinConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_NoughtPlayerGameWinConditions_GameId",
                table: "NoughtPlayerGameWinConditions",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_NoughtPlayerGameWinConditions_WinConditionId",
                table: "NoughtPlayerGameWinConditions",
                column: "WinConditionId");
        }
    }
}
