using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DiplomaProject.Domain.Migrations
{
    public partial class UserRoleProfession : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Professions_AspNetUsers_AdminId",
                table: "Professions");

            migrationBuilder.DropIndex(
                name: "IX_Professions_AdminId",
                table: "Professions");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Professions");

            migrationBuilder.AlterColumn<float>(
                name: "Weight",
                table: "OutComes",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AddColumn<int>(
                name: "ProfessionId",
                table: "AspNetUserRoles",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_ProfessionId",
                table: "AspNetUserRoles",
                column: "ProfessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_Professions_ProfessionId",
                table: "AspNetUserRoles",
                column: "ProfessionId",
                principalTable: "Professions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_Professions_ProfessionId",
                table: "AspNetUserRoles");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserRoles_ProfessionId",
                table: "AspNetUserRoles");

            migrationBuilder.DropColumn(
                name: "ProfessionId",
                table: "AspNetUserRoles");

            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "Professions",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Weight",
                table: "OutComes",
                nullable: false,
                oldClrType: typeof(float));

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
    }
}
