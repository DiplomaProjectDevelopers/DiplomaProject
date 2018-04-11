using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DiplomaProject.Domain.Migrations
{
    public partial class BranchProfession : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "Professions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Professions_BranchId",
                table: "Professions",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Professions_Branches_BranchId",
                table: "Professions",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Professions_Branches_BranchId",
                table: "Professions");

            migrationBuilder.DropIndex(
                name: "IX_Professions_BranchId",
                table: "Professions");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Professions");
        }
    }
}
