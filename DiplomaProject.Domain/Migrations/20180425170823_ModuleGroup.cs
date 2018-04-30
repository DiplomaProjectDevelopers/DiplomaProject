using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DiplomaProject.Domain.Migrations
{
    public partial class ModuleGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InitialOutComes_InitialSubjects_SubjectId",
                table: "InitialOutComes");

            migrationBuilder.DropForeignKey(
                name: "FK_InitialOutComes_OutComeTypes_TypeId",
                table: "InitialOutComes");

            migrationBuilder.DropForeignKey(
                name: "FK_OutComes_StakeHolders_StakeholderId",
                table: "OutComes");

            migrationBuilder.DropForeignKey(
                name: "FK_OutComes_InitialSubjects_SubjectId",
                table: "OutComes");

            migrationBuilder.DropForeignKey(
                name: "FK_OutComes_OutComeTypes_TypeId",
                table: "OutComes");

            migrationBuilder.RenameColumn(
                name: "StakeholderId",
                table: "OutComes",
                newName: "StakeHolderId");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "OutComes",
                newName: "OutComeTypeId");

            migrationBuilder.RenameColumn(
                name: "SubjectId",
                table: "OutComes",
                newName: "InitialSubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_OutComes_StakeholderId",
                table: "OutComes",
                newName: "IX_OutComes_StakeHolderId");

            migrationBuilder.RenameIndex(
                name: "IX_OutComes_TypeId",
                table: "OutComes",
                newName: "IX_OutComes_OutComeTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_OutComes_SubjectId",
                table: "OutComes",
                newName: "IX_OutComes_InitialSubjectId");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "InitialOutComes",
                newName: "OutComeTypeId");

            migrationBuilder.RenameColumn(
                name: "SubjectId",
                table: "InitialOutComes",
                newName: "InitialSubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_InitialOutComes_TypeId",
                table: "InitialOutComes",
                newName: "IX_InitialOutComes_OutComeTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_InitialOutComes_SubjectId",
                table: "InitialOutComes",
                newName: "IX_InitialOutComes_InitialSubjectId");

            migrationBuilder.AddColumn<string>(
                name: "Group",
                table: "SubjectModules",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_InitialOutComes_InitialSubjects_InitialSubjectId",
                table: "InitialOutComes",
                column: "InitialSubjectId",
                principalTable: "InitialSubjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InitialOutComes_OutComeTypes_OutComeTypeId",
                table: "InitialOutComes",
                column: "OutComeTypeId",
                principalTable: "OutComeTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OutComes_InitialSubjects_InitialSubjectId",
                table: "OutComes",
                column: "InitialSubjectId",
                principalTable: "InitialSubjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OutComes_OutComeTypes_OutComeTypeId",
                table: "OutComes",
                column: "OutComeTypeId",
                principalTable: "OutComeTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OutComes_StakeHolders_StakeHolderId",
                table: "OutComes",
                column: "StakeHolderId",
                principalTable: "StakeHolders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InitialOutComes_InitialSubjects_InitialSubjectId",
                table: "InitialOutComes");

            migrationBuilder.DropForeignKey(
                name: "FK_InitialOutComes_OutComeTypes_OutComeTypeId",
                table: "InitialOutComes");

            migrationBuilder.DropForeignKey(
                name: "FK_OutComes_InitialSubjects_InitialSubjectId",
                table: "OutComes");

            migrationBuilder.DropForeignKey(
                name: "FK_OutComes_OutComeTypes_OutComeTypeId",
                table: "OutComes");

            migrationBuilder.DropForeignKey(
                name: "FK_OutComes_StakeHolders_StakeHolderId",
                table: "OutComes");

            migrationBuilder.DropColumn(
                name: "Group",
                table: "SubjectModules");

            migrationBuilder.RenameColumn(
                name: "StakeHolderId",
                table: "OutComes",
                newName: "StakeholderId");

            migrationBuilder.RenameColumn(
                name: "OutComeTypeId",
                table: "OutComes",
                newName: "TypeId");

            migrationBuilder.RenameColumn(
                name: "InitialSubjectId",
                table: "OutComes",
                newName: "SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_OutComes_StakeHolderId",
                table: "OutComes",
                newName: "IX_OutComes_StakeholderId");

            migrationBuilder.RenameIndex(
                name: "IX_OutComes_OutComeTypeId",
                table: "OutComes",
                newName: "IX_OutComes_TypeId");

            migrationBuilder.RenameIndex(
                name: "IX_OutComes_InitialSubjectId",
                table: "OutComes",
                newName: "IX_OutComes_SubjectId");

            migrationBuilder.RenameColumn(
                name: "OutComeTypeId",
                table: "InitialOutComes",
                newName: "TypeId");

            migrationBuilder.RenameColumn(
                name: "InitialSubjectId",
                table: "InitialOutComes",
                newName: "SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_InitialOutComes_OutComeTypeId",
                table: "InitialOutComes",
                newName: "IX_InitialOutComes_TypeId");

            migrationBuilder.RenameIndex(
                name: "IX_InitialOutComes_InitialSubjectId",
                table: "InitialOutComes",
                newName: "IX_InitialOutComes_SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_InitialOutComes_InitialSubjects_SubjectId",
                table: "InitialOutComes",
                column: "SubjectId",
                principalTable: "InitialSubjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InitialOutComes_OutComeTypes_TypeId",
                table: "InitialOutComes",
                column: "TypeId",
                principalTable: "OutComeTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OutComes_StakeHolders_StakeholderId",
                table: "OutComes",
                column: "StakeholderId",
                principalTable: "StakeHolders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OutComes_InitialSubjects_SubjectId",
                table: "OutComes",
                column: "SubjectId",
                principalTable: "InitialSubjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OutComes_OutComeTypes_TypeId",
                table: "OutComes",
                column: "TypeId",
                principalTable: "OutComeTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
