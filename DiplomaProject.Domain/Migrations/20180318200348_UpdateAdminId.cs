using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DiplomaProject.Domain.Migrations
{
    public partial class UpdateAdminId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Professions_AspNetUsers_AdminId1",
                table: "Professions");

            migrationBuilder.DropIndex(
                name: "IX_Professions_AdminId1",
                table: "Professions");

            migrationBuilder.DropColumn(
                name: "AdminId1",
                table: "Professions");

            migrationBuilder.AlterColumn<string>(
                name: "AdminId",
                table: "Professions",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Professions_AdminId",
                table: "Professions",
                column: "AdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_Professions_AspNetUsers_AdminId",
                table: "Professions",
                column: "AdminId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Professions_AspNetUsers_AdminId",
                table: "Professions");

            migrationBuilder.DropIndex(
                name: "IX_Professions_AdminId",
                table: "Professions");

            migrationBuilder.AlterColumn<int>(
                name: "AdminId",
                table: "Professions",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminId1",
                table: "Professions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Professions_AdminId1",
                table: "Professions",
                column: "AdminId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Professions_AspNetUsers_AdminId1",
                table: "Professions",
                column: "AdminId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
