using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDo.Migrations
{
    public partial class UpdateNoteCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "date",
                table: "Note",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "content",
                table: "Note",
                newName: "Content");

            migrationBuilder.RenameColumn(
                name: "completed",
                table: "Note",
                newName: "Completed");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Note",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Category",
                table: "Note",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Note",
                newName: "date");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Note",
                newName: "content");

            migrationBuilder.RenameColumn(
                name: "Completed",
                table: "Note",
                newName: "completed");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Note",
                newName: "id");

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "Note",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
