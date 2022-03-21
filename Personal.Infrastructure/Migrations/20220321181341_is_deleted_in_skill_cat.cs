using Microsoft.EntityFrameworkCore.Migrations;

namespace Personal.Infrastructure.Migrations
{
    public partial class is_deleted_in_skill_cat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ResumeSkillCategories",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ResumeSkillCategories");
        }
    }
}
