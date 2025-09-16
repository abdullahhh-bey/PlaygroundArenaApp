using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlaygroundArenaApp.Migrations
{
    /// <inheritdoc />
    public partial class ChangeStartEndTimeToTimeSpan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeSlots_Users_UserId",
                table: "TimeSlots");

            migrationBuilder.DropIndex(
                name: "IX_TimeSlots_UserId",
                table: "TimeSlots");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TimeSlots");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "StartTime",
                table: "Bookings",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "EndTime",
                table: "Bookings",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "TimeSlots",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartTime",
                table: "Bookings",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndTime",
                table: "Bookings",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.CreateIndex(
                name: "IX_TimeSlots_UserId",
                table: "TimeSlots",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeSlots_Users_UserId",
                table: "TimeSlots",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }
    }
}
