using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce_App.Migrations
{
    /// <inheritdoc />
    public partial class EditColumne : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "AspNetUsers",
                newName: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "AspNetUsers",
                newName: "IsDeleted");
        }
    }
}
