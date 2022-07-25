using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OfficeManager.Infrastructure.Migrations
{
    public partial class addedProfileToIdentityUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "UserProfile",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfilePic",
                table: "UserProfile",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProfileId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ProfileId",
                table: "AspNetUsers",
                column: "ProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_UserProfile_ProfileId",
                table: "AspNetUsers",
                column: "ProfileId",
                principalTable: "UserProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_UserProfile_ProfileId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ProfileId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "UserProfile");

            migrationBuilder.DropColumn(
                name: "ProfilePic",
                table: "UserProfile");

            migrationBuilder.DropColumn(
                name: "ProfileId",
                table: "AspNetUsers");
        }
    }
}
