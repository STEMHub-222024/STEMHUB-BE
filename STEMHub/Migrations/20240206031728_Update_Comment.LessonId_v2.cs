using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace STEMHub.Migrations
{
    public partial class Update_CommentLessonId_v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Lesson_LessonId1",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Comment_LessonId1",
                table: "Comment");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8e6a70a1-ab45-40de-afd6-35f52b008cbc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "de950809-d67a-4d9c-9c69-945d881ba002");

            migrationBuilder.DropColumn(
                name: "LessonId1",
                table: "Comment");

            migrationBuilder.AlterColumn<Guid>(
                name: "LessonId",
                table: "Comment",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "161965f6-ab0d-42f5-93c9-d5fad0f9b9ea", "2", "User", "User" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "93d1019b-a93f-4e18-bdfe-2eb1c9c254bc", "1", "Admin", "Admin" });

            migrationBuilder.CreateIndex(
                name: "IX_Comment_LessonId",
                table: "Comment",
                column: "LessonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Lesson_LessonId",
                table: "Comment",
                column: "LessonId",
                principalTable: "Lesson",
                principalColumn: "LessonId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Lesson_LessonId",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Comment_LessonId",
                table: "Comment");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "161965f6-ab0d-42f5-93c9-d5fad0f9b9ea");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "93d1019b-a93f-4e18-bdfe-2eb1c9c254bc");

            migrationBuilder.AlterColumn<string>(
                name: "LessonId",
                table: "Comment",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "LessonId1",
                table: "Comment",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "8e6a70a1-ab45-40de-afd6-35f52b008cbc", "2", "User", "User" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "de950809-d67a-4d9c-9c69-945d881ba002", "1", "Admin", "Admin" });

            migrationBuilder.CreateIndex(
                name: "IX_Comment_LessonId1",
                table: "Comment",
                column: "LessonId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Lesson_LessonId1",
                table: "Comment",
                column: "LessonId1",
                principalTable: "Lesson",
                principalColumn: "LessonId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
