using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DiplomaProject.Domain.Migrations
{
    public partial class RoleDisplayName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InitialSubjectId",
                table: "FinalOutComes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "AspNetRoles",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FinalOutComes_InitialSubjectId",
                table: "FinalOutComes",
                column: "InitialSubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_FinalOutComes_InitialSubjects_InitialSubjectId",
                table: "FinalOutComes",
                column: "InitialSubjectId",
                principalTable: "InitialSubjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinalOutComes_InitialSubjects_InitialSubjectId",
                table: "FinalOutComes");

            migrationBuilder.DropIndex(
                name: "IX_FinalOutComes_InitialSubjectId",
                table: "FinalOutComes");

            migrationBuilder.DropColumn(
                name: "InitialSubjectId",
                table: "FinalOutComes");

            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "AspNetRoles");
        }
    }
}
