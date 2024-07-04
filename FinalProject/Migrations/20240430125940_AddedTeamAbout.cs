using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProject.Migrations
{
    /// <inheritdoc />
    public partial class AddedTeamAbout : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeamMemberDescription",
                table: "TeamAbouts");

            migrationBuilder.DropColumn(
                name: "TeamMemberName",
                table: "TeamAbouts");

            migrationBuilder.DropColumn(
                name: "TeamMemberPosition",
                table: "TeamAbouts");

            migrationBuilder.DropColumn(
                name: "TeamTitle",
                table: "TeamAbouts");

            migrationBuilder.DropColumn(
                name: "profileImageUrl",
                table: "TeamAbouts");

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    profileImageUrl = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    TeamMemberName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TeamMemberDescription = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TeamMemberPosition = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.AddColumn<string>(
                name: "TeamMemberDescription",
                table: "TeamAbouts",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TeamMemberName",
                table: "TeamAbouts",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TeamMemberPosition",
                table: "TeamAbouts",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TeamTitle",
                table: "TeamAbouts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "profileImageUrl",
                table: "TeamAbouts",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");
        }
    }
}
