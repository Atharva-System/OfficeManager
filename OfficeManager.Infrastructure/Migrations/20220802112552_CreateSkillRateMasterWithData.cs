using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OfficeManager.Infrastructure.Migrations
{
    public partial class CreateSkillRateMasterWithData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SkillRate",
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
                    table.PrimaryKey("PK_SkillRate", x => x.Id);
                });

            migrationBuilder.InsertData(
             table: "SkillRate",
             columns: new[] { "Name", "Description", "IsActive" },
             values: new object[,] {
                    {"1","1", true },
                    {"2","2",true },
                    {"3","3",true },
                    {"4","4",true },
                    {"5","5",true },
                    {"6","6",true },
                    {"7","7",true },
                    {"8","8",true },
                    {"9","9",true },
                    {"10","10",true }
             });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SkillRate");
        }
    }
}
