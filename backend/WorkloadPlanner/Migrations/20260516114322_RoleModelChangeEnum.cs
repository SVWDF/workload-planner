using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkloadPlanner.Migrations
{
    /// <inheritdoc />
    public partial class RoleModelChangeEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "BoardMembers",
                type: "nvarchar(20)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "BoardMembers",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
