using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace STEMHub.Migrations
{
    public partial class update_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0976abd4-fcd5-4891-8aec-5db501c978f3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a751699d-d249-4f8c-bd28-ec737bb59bf5");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "NewspaperArticle",
                newName: "Description_NA");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "484c7d32-11df-46c8-9f02-5066e90a2cde", "1", "Admin", "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a051528f-86bd-48da-9628-f8287e88ea1c", "2", "User", "User" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "484c7d32-11df-46c8-9f02-5066e90a2cde");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a051528f-86bd-48da-9628-f8287e88ea1c");

            migrationBuilder.RenameColumn(
                name: "Description_NA",
                table: "NewspaperArticle",
                newName: "Description");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "0976abd4-fcd5-4891-8aec-5db501c978f3", "1", "Admin", "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a751699d-d249-4f8c-bd28-ec737bb59bf5", "2", "User", "User" });
        }
    }
}
