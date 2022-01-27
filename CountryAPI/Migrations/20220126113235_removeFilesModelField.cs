using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CountryAPI.Migrations
{
    public partial class removeFilesModelField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Url",
                table: "Files",
                newName: "FileName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "Files",
                newName: "Url");
        }
    }
}
