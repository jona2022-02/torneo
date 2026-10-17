using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Torneo.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPlayerLineupRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.Sql("""
                UPDATE Players
                SET Role = CASE ShirtNumber
                    WHEN 1 THEN 0
                    WHEN 4 THEN 2
                    WHEN 5 THEN 3
                    ELSE 1
                END
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "Players");
        }
    }
}
