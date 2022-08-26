using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OfficeManager.Infrastructure.Migrations
{
    public partial class connectedDepartmentDesignationWithEmployee2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "DesignationId",
                table: "Employees",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "Employees",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_DepartMent_DepartmentID",
                table: "Employees",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade
                );

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Designation_DesignationID",
                table: "Employees",
                column: "DesignationId",
                principalTable: "Designation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade
                );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "DesignationId",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_DepartMent_DepartmentID",
                table: "Employees"
                );

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Designation_DesignationID",
                table: "Employees"
                );
        }
    }
}
