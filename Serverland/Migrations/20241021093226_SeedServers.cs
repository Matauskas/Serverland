using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Serverland.Migrations
{
    /// <inheritdoc />
    public partial class SeedServers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Servers",
                columns: new[] { "Id", "Disk_Count", "Generation", "Model", "OS", "Weight", "categoryId" },
                values: new object[,]
                {
                    { 1, 2, "ugXGzF", "Hdzl5a8hdf6", true, 12.5, 1 },
                    { 2, 4, "GenX", "Xyz5000", false, 18.699999999999999, 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Servers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Servers",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
