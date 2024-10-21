using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Serverland.Migrations
{
    /// <inheritdoc />
    public partial class SeedParts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Parts",
                columns: new[] { "Id", "CPU", "HDD", "Network", "PSU", "RAM", "Raid", "Rails", "SSD", "serverId" },
                values: new object[,]
                {
                    { 1, "Intel Xeon E5-2620", "2TB", "1Gbps Ethernet", "750W", "32GB DDR4", "RAID 0/1/5/10", true, "512GB", 1 },
                    { 2, "Intel Xeon E5-2670", "4TB", "1Gbps Ethernet", "850W", "64GB DDR4", "RAID 5/6", false, "1TB", 1 },
                    { 3, "AMD EPYC 7302P", "8TB", "10Gbps Ethernet", "1000W", "128GB DDR4", "RAID 10", true, "2TB", 2 },
                    { 4, "Intel Core i9-10900X", "1TB", "1Gbps Ethernet", "650W", "64GB DDR4", "RAID 0/1", true, "512GB", 2 },
                    { 5, "AMD Ryzen Threadripper 3990X", "5TB", "10Gbps Ethernet", "1200W", "256GB DDR4", "RAID 0/1/5", false, "4TB", 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
