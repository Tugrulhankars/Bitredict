using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bitredict.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HomePageStatistics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PredictedToday = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BankersRate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MathcesWon = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomePageStatistics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MatchCenterStatistics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Predict = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Upcomig = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Won = table.Column<string>(type: "nvarchar(max)", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchCenterStatistics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Matchs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    League = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BetOfTheDayStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MatchStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Teams = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MatchDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Odds1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OddsX = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Odds2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tip = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipOdd = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Goals = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GoalsOdd = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gg = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GgOdd = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BestTip = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BestTipOdd = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Trust = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateOnly>(type: "date", nullable: false),
                    UpdatedDate = table.Column<DateOnly>(type: "date", nullable: true),
                    DeletedDate = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matchs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WalletPublicAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateOnly>(type: "date", nullable: false),
                    UpdatedDate = table.Column<DateOnly>(type: "date", nullable: true),
                    DeletedDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ActiveStartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ActiveFinishDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HomePageStatistics");

            migrationBuilder.DropTable(
                name: "MatchCenterStatistics");

            migrationBuilder.DropTable(
                name: "Matchs");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
