using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OfficeManager.Infrastructure.Migrations
{
    public partial class departmentdesignationremovedfromusermaster : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserMaster_DepartmentMasters_DepartmentId",
                table: "UserMaster");

            migrationBuilder.DropForeignKey(
                name: "FK_UserMaster_DesignationMasters_DesignationId",
                table: "UserMaster");

            migrationBuilder.DropIndex(
                name: "IX_UserMaster_DepartmentId",
                table: "UserMaster");

            migrationBuilder.DropIndex(
                name: "IX_UserMaster_DesignationId",
                table: "UserMaster");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "UserMaster");

            migrationBuilder.DropColumn(
                name: "DesignationId",
                table: "UserMaster");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentId",
                table: "UserMaster",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DesignationId",
                table: "UserMaster",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_UserMaster_DepartmentId",
                table: "UserMaster",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMaster_DesignationId",
                table: "UserMaster",
                column: "DesignationId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserMaster_DepartmentMasters_DepartmentId",
                table: "UserMaster",
                column: "DepartmentId",
                principalTable: "DepartmentMasters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserMaster_DesignationMasters_DesignationId",
                table: "UserMaster",
                column: "DesignationId",
                principalTable: "DesignationMasters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
