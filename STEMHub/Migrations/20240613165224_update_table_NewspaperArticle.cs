using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace STEMHub.Migrations
{
    public partial class update_table_NewspaperArticle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewspaperArticle_AspNetUsers_UserId",
                table: "NewspaperArticle");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "05897726-a55e-4543-a91f-12608d08c378");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0d6db2c3-a0e7-4c71-b1cf-76455713bd32");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Video",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VideoReview",
                table: "Topic",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "NewspaperArticle",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "NewspaperArticle",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "NewspaperArticle",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "create_at",
                table: "NewspaperArticle",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "0976abd4-fcd5-4891-8aec-5db501c978f3", "1", "Admin", "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a751699d-d249-4f8c-bd28-ec737bb59bf5", "2", "User", "User" });

            migrationBuilder.AddForeignKey(
                name: "FK_NewspaperArticle_AspNetUsers_UserId",
                table: "NewspaperArticle",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewspaperArticle_AspNetUsers_UserId",
                table: "NewspaperArticle");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0976abd4-fcd5-4891-8aec-5db501c978f3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a751699d-d249-4f8c-bd28-ec737bb59bf5");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Video");

            migrationBuilder.DropColumn(
                name: "VideoReview",
                table: "Topic");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "NewspaperArticle");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "NewspaperArticle");

            migrationBuilder.DropColumn(
                name: "create_at",
                table: "NewspaperArticle");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "NewspaperArticle",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "05897726-a55e-4543-a91f-12608d08c378", "2", "User", "User" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "0d6db2c3-a0e7-4c71-b1cf-76455713bd32", "1", "Admin", "Admin" });

            migrationBuilder.AddForeignKey(
                name: "FK_NewspaperArticle_AspNetUsers_UserId",
                table: "NewspaperArticle",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
