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
                keyValue: "29d2228e-f4ad-47b0-99fe-1291f4ce6ea0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3766f3c3-3d24-470d-a001-547bdda6a206");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f7af1a29-b696-4ade-b2a6-676c0ce67d54");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayedUsername",
                table: "UserProfiles",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4b9709ab-df7f-40a0-bb2c-1a0e46e1229d", "3", "HR", "HR" },
                    { "96dfd7a3-3c6c-4d18-a319-7aafe15f81ac", "1", "Admin", "Admin" },
                    { "e6ea9c18-48bd-4153-bce0-97408212e119", "2", "User", "User" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_DisplayedUsername",
                table: "UserProfiles",
                column: "DisplayedUsername",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserProfiles_DisplayedUsername",
                table: "UserProfiles");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4b9709ab-df7f-40a0-bb2c-1a0e46e1229d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "96dfd7a3-3c6c-4d18-a319-7aafe15f81ac");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e6ea9c18-48bd-4153-bce0-97408212e119");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayedUsername",
                table: "UserProfiles",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "29d2228e-f4ad-47b0-99fe-1291f4ce6ea0", "3", "HR", "HR" },
                    { "3766f3c3-3d24-470d-a001-547bdda6a206", "2", "User", "User" },
                    { "f7af1a29-b696-4ade-b2a6-676c0ce67d54", "1", "Admin", "Admin" }
                });
        }
    }
}
