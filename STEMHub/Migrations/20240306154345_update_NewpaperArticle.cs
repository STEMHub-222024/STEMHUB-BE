using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace STEMHub.Migrations
{
    public partial class update_NewpaperArticle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2b10851b-54b1-4625-a6c1-890792be3789");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9f1ace67-6696-4db0-b164-95ec184b238a");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "NewspaperArticle",
                newName: "Markdown");

            migrationBuilder.RenameColumn(
                name: "Content_NA",
                table: "NewspaperArticle",
                newName: "HtmlContent");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "3095b3e8-c939-4632-a721-2debe8561bcc", "2", "User", "User" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "61febf14-678b-4521-88b1-3c843016e113", "1", "Admin", "Admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3095b3e8-c939-4632-a721-2debe8561bcc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "61febf14-678b-4521-88b1-3c843016e113");

            migrationBuilder.RenameColumn(
                name: "Markdown",
                table: "NewspaperArticle",
                newName: "Image");

            migrationBuilder.RenameColumn(
                name: "HtmlContent",
                table: "NewspaperArticle",
                newName: "Content_NA");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "2b10851b-54b1-4625-a6c1-890792be3789", "2", "User", "User" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "9f1ace67-6696-4db0-b164-95ec184b238a", "1", "Admin", "Admin" });
        }
    }
}
