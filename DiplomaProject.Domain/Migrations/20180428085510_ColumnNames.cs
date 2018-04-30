using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DiplomaProject.Domain.Migrations
{
    public partial class ColumnNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModuleId",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "FinalOutComes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ModuleId",
                table: "Subjects",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "FinalOutComes",
                nullable: true);
        }
    }
}
