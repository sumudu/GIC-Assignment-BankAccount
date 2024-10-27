using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AwesomeGIC.Migrations
{
    /// <inheritdoc />
    public partial class ModifyInterestRule_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "InterestRules",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "InterestRules");
        }
    }
}
