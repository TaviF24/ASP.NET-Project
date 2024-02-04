using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Project.Migrations
{
    /// <inheritdoc />
    public partial class FirstCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0daecd1f-3f5e-4312-b778-bf993d86943d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "679dd71e-0b94-4f53-9d92-0f9c7520e792");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dc330a13-4577-4957-bfee-ccee62dafc34");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "407edf89-9b69-406a-8324-441b10c59ce1", "3", "HR", "HR" },
                    { "65260eba-0fc5-4e80-8f57-aa95246a66ab", "1", "Admin", "Admin" },
                    { "78fcc00a-6760-4cb7-964b-2797699e679c", "2", "User", "User" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "407edf89-9b69-406a-8324-441b10c59ce1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "65260eba-0fc5-4e80-8f57-aa95246a66ab");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "78fcc00a-6760-4cb7-964b-2797699e679c");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0daecd1f-3f5e-4312-b778-bf993d86943d", "2", "User", "User" },
                    { "679dd71e-0b94-4f53-9d92-0f9c7520e792", "3", "HR", "HR" },
                    { "dc330a13-4577-4957-bfee-ccee62dafc34", "1", "Admin", "Admin" }
                });
        }
    }
}
