using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OfficeManager.Infrastructure.Migrations
{
    public partial class CreateDesignationMasterWithData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Designation",
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
                    table.PrimaryKey("PK_Designation", x => x.Id);
                });
            migrationBuilder.InsertData(
            table: "Designation",
            columns: new[] { "Name", "Description", "IsActive" },
            values: new object[,] {
                    {"Business development executive","Business development executive", true },
                    {"Business Development Manager","Business Development Manager", true },
                    {"CEO","CEO", true },
                    {"CTO","CTO", true },
                    {"Data Analyst","Data Analyst", true },
                    {"DIGITAL MARKETING  EXECUTIVE","DIGITAL MARKETING  EXECUTIVE", true },
                    {"HR Executive","HR Executive", true },
                    {"Jr. Business Development Executive","Jr. Business Development Executive", true },
                    {"Jr. DevOps Engineer","Jr. DevOps Engineer", true },
                    {"Jr. QA","Jr. QA", true },
                    {"Jr. Software Engineer","Jr. Software Engineer", true },
                    {"Jr. UI/UX Engineer","Jr. UI/UX Engineer", true },
                    {"Jr. Odoo Developer","Jr. Odoo Developer", true },
                    {"Project manager","Project manager", true },
                    {"QA Engineer","QA Engineer", true },
                    {"Sales Head","Sales Head", true },
                    {"Senior Software Engineer","Senior Software Engineer", true },
                    {"Senior System Administrator","Senior System Administrator", true },
                    {"Shopify Developer","Shopify Developer", true },
                    {"Software Engineer","Software Engineer", true },
                    {"Sr. Data Analyst (Power BI)","Sr. Data Analyst (Power BI)", true },
                    {"Sr. DevOps Engineer","Sr. DevOps Engineer", true },
                    {"Sr. HR Manager","Sr. HR Manager", true },
                    {"Sr. Magento Developer","Sr. Magento Developer", true },
                    {"Sr. QA Engineer","Sr. QA Engineer", true },
                    {"Sr. Software Engineer","Sr. Software Engineer", true },
                    {"Sr. UI/UX Engineer","Sr. UI/UX Engineer", true },
                    {"Team Lead","Team Lead", true }
            });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Designation");
        }
    }
}
