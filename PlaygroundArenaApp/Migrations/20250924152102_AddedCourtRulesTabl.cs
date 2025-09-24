using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlaygroundArenaApp.Migrations
{
    /// <inheritdoc />
    public partial class AddedCourtRulesTabl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CourtRules",
                columns: table => new
                {
                    RuleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourtId = table.Column<int>(type: "int", nullable: false),
                    TimeInterval = table.Column<int>(type: "int", nullable: false),
                    MinimumSlotsBooking = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourtRules", x => x.RuleId);
                    table.ForeignKey(
                        name: "FK_CourtRules_Courts_CourtId",
                        column: x => x.CourtId,
                        principalTable: "Courts",
                        principalColumn: "CourtId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourtRules_CourtId",
                table: "CourtRules",
                column: "CourtId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourtRules");
        }
    }
}
