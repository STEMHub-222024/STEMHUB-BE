using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace STEMHub.Migrations
{
    public partial class update_field_table_scientist_ : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContentMarkdown",
                table: "Scientist",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionScientist",
                table: "Scientist",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentMarkdown",
                table: "Scientist");

            migrationBuilder.DropColumn(
                name: "DescriptionScientist",
                table: "Scientist");
        }
    }
}
