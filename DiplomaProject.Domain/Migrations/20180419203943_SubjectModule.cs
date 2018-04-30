using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DiplomaProject.Domain.Migrations
{
    public partial class SubjectModule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ModuleId",
                table: "Subjects",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubjectModuleId",
                table: "Subjects",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SubjectModules",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectModules", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_SubjectModuleId",
                table: "Subjects",
                column: "SubjectModuleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_SubjectModules_SubjectModuleId",
                table: "Subjects",
                column: "SubjectModuleId",
                principalTable: "SubjectModules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_SubjectModules_SubjectModuleId",
                table: "Subjects");

            migrationBuilder.DropTable(
                name: "SubjectModules");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_SubjectModuleId",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "ModuleId",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "SubjectModuleId",
                table: "Subjects");
        }
    }
}
