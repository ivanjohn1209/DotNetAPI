using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CountryAPI.Migrations
{
    public partial class UpdateNewFileModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataFiles",
                table: "Files");

            migrationBuilder.AddColumn<Guid>(
                name: "ref_assignto",
                table: "Files",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ref_file",
                table: "Files",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ref_assignto",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "ref_file",
                table: "Files");

            migrationBuilder.AddColumn<byte[]>(
                name: "DataFiles",
                table: "Files",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
