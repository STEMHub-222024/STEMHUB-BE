using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace STEMHub.Migrations
{
    public partial class update_video : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Video_Lesson_LessonId",
                table: "Video");

            migrationBuilder.DropIndex(
                name: "IX_Video_LessonId",
                table: "Video");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4cfe1dde-f34f-4ea0-9b5c-82c4a13be818");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f6370971-9c0e-4b81-9bbe-68615c328a8f");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "4f56c400-7ea2-44cd-b4e9-ca84678380a1", "1", "Admin", "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "796fd124-8992-4d1e-8daf-18f6f2a454a9", "2", "User", "User" });

            migrationBuilder.CreateIndex(
                name: "IX_Video_LessonId",
                table: "Video",
                column: "LessonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Video_Lesson_LessonId",
                table: "Video",
                column: "LessonId",
                principalTable: "Lesson",
                principalColumn: "LessonId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Video_Lesson_LessonId",
                table: "Video");

            migrationBuilder.DropIndex(
                name: "IX_Video_LessonId",
                table: "Video");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4f56c400-7ea2-44cd-b4e9-ca84678380a1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "796fd124-8992-4d1e-8daf-18f6f2a454a9");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "4cfe1dde-f34f-4ea0-9b5c-82c4a13be818", "2", "User", "User" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f6370971-9c0e-4b81-9bbe-68615c328a8f", "1", "Admin", "Admin" });

            migrationBuilder.CreateIndex(
                name: "IX_Video_LessonId",
                table: "Video",
                column: "LessonId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Video_Lesson_LessonId",
                table: "Video",
                column: "LessonId",
                principalTable: "Lesson",
                principalColumn: "LessonId");
        }
    }
}
