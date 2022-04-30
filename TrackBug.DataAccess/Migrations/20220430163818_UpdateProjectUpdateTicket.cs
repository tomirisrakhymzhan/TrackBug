using Microsoft.EntityFrameworkCore.Migrations;

namespace TrackBug.DataAccess.Migrations
{
    public partial class UpdateProjectUpdateTicket : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShortDescription",
                table: "Projects",
                type: "TEXT",
                maxLength: 30,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShortDescription",
                table: "Projects");
        }
    }
}
