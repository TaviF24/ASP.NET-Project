using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Project.Migrations
{
    /// <inheritdoc />
    public partial class SecondCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c9a2c87-fd4f-4a8b-8879-adc0ac2af86e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9c594fe1-638e-4d77-8de9-1712ca25831e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ac6672a8-e19e-4302-886f-c84808660976");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "053529c2-5152-442d-9c31-7bf43d13c2d8", "1", "Admin", "Admin" },
                    { "93697d90-d92e-4a4b-83a0-e0d610cffc07", "2", "User", "User" },
                    { "a759ab0d-0b83-4a49-bd2b-860fd2a14b52", "3", "HR", "HR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "053529c2-5152-442d-9c31-7bf43d13c2d8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "93697d90-d92e-4a4b-83a0-e0d610cffc07");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a759ab0d-0b83-4a49-bd2b-860fd2a14b52");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2c9a2c87-fd4f-4a8b-8879-adc0ac2af86e", "3", "HR", "HR" },
                    { "9c594fe1-638e-4d77-8de9-1712ca25831e", "1", "Admin", "Admin" },
                    { "ac6672a8-e19e-4302-886f-c84808660976", "2", "User", "User" }
                });
        }
    }
}
