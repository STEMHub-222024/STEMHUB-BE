using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace STEMHub.Migrations
{
    public partial class add_field_table_part : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PartName",
                table: "Parts",
                newName: "StepsMarkdown");

            migrationBuilder.AddColumn<string>(
                name: "MaterialsHtmlContent",
                table: "Parts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MaterialsMarkdown",
                table: "Parts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ResultsHtmlContent",
                table: "Parts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ResultsMarkdown",
                table: "Parts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StepsHtmlContent",
                table: "Parts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaterialsHtmlContent",
                table: "Parts");

            migrationBuilder.DropColumn(
                name: "MaterialsMarkdown",
                table: "Parts");

            migrationBuilder.DropColumn(
                name: "ResultsHtmlContent",
                table: "Parts");

            migrationBuilder.DropColumn(
                name: "ResultsMarkdown",
                table: "Parts");

            migrationBuilder.DropColumn(
                name: "StepsHtmlContent",
                table: "Parts");

            migrationBuilder.RenameColumn(
                name: "StepsMarkdown",
                table: "Parts",
                newName: "PartName");
        }
    }
}
