using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace STEMHub.Migrations
{
    public partial class update_Topic__addView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3095b3e8-c939-4632-a721-2debe8561bcc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "61febf14-678b-4521-88b1-3c843016e113");

            migrationBuilder.AddColumn<int>(
                name: "View",
                table: "Topic",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "13fec886-f124-4e6f-a52d-321942cb4976", "2", "User", "User" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "39fdac39-aa21-4e65-899d-85f9f4580361", "1", "Admin", "Admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "13fec886-f124-4e6f-a52d-321942cb4976");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "39fdac39-aa21-4e65-899d-85f9f4580361");

            migrationBuilder.DropColumn(
                name: "View",
                table: "Topic");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "3095b3e8-c939-4632-a721-2debe8561bcc", "2", "User", "User" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "61febf14-678b-4521-88b1-3c843016e113", "1", "Admin", "Admin" });
        }
    }
}
