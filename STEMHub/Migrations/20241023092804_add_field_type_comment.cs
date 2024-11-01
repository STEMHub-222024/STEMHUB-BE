using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace STEMHub.Migrations
{
    public partial class add_field_type_comment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Lesson_LessonId",
                table: "Comment");

            migrationBuilder.AlterColumn<Guid>(
                name: "LessonId",
                table: "Comment",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "NewspaperArticleId",
                table: "Comment",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Comment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_NewspaperArticleId",
                table: "Comment",
                column: "NewspaperArticleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Lesson_LessonId",
                table: "Comment",
                column: "LessonId",
                principalTable: "Lesson",
                principalColumn: "LessonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_NewspaperArticle_NewspaperArticleId",
                table: "Comment",
                column: "NewspaperArticleId",
                principalTable: "NewspaperArticle",
                principalColumn: "NewspaperArticleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Lesson_LessonId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Comment_NewspaperArticle_NewspaperArticleId",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Comment_NewspaperArticleId",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "NewspaperArticleId",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Comment");

            migrationBuilder.AlterColumn<Guid>(
                name: "LessonId",
                table: "Comment",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Lesson_LessonId",
                table: "Comment",
                column: "LessonId",
                principalTable: "Lesson",
                principalColumn: "LessonId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
