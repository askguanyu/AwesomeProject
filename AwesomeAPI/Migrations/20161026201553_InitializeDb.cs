using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AwesomeAPI.Migrations
{
    public partial class InitializeDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActivityType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    Description = table.Column<string>(maxLength: 50, nullable: false),
                    InsertedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "DATETIME()"),
                    UpdatedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "DATETIME()"),
                    UserId = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Activity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    ActivityTypeId = table.Column<int>(nullable: false),
                    Details = table.Column<string>(maxLength: 255, nullable: true),
                    End = table.Column<DateTime>(nullable: true),
                    InsertedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "DATETIME()"),
                    Start = table.Column<DateTime>(nullable: false),
                    Subject = table.Column<string>(maxLength: 50, nullable: false),
                    UpdatedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "DATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Activity_ActivityType_ActivityTypeId",
                        column: x => x.ActivityTypeId,
                        principalTable: "ActivityType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activity_ActivityTypeId",
                table: "Activity",
                column: "ActivityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityType_Description",
                table: "ActivityType",
                column: "Description",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Activity");

            migrationBuilder.DropTable(
                name: "ActivityType");
        }
    }
}
