using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OfficeManager.Infrastructure.Migrations
{
    public partial class CreateDepartmentMasterWithData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.Id);
                });

            migrationBuilder.InsertData(
            table: "Department",
            columns: new[] { "Name", "Description", "IsActive" },
            values: new object[,] {
                    {".Net",".Net", true },
                    {"Analytics","Analytics",true },
                    {"Design","Design",true },
                    {"Digital Marketing","Digital Marketing",true },
                    {"HR","HR",true },
                    {"Information Security","Information Security",true },
                    {"Magento","Magento",true },
                    {"Management","Management",true },
                    {"Mobile","Mobile",true },
                    {"Odoo","Odoo",true },
                    {"PHP","PHP",true },
                    {"QA","QA",true },
                    {"React","React",true },
                    {"ROR","ROR",true },
                    {"Sales","Sales",true },
                    {"Sales & Marketing","Sales & Marketing",true },
                    {"Shopify","Shopify",true },
                    {"SRE","SRE",true }
            });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Department");
        }
    }
}
