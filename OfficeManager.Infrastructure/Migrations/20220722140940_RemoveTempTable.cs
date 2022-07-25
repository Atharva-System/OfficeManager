using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OfficeManager.Infrastructure.Migrations
{
    public partial class RemoveTempTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TempTable");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
