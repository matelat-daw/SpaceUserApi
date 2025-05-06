using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SpaceUserAPI.Migrations
{
    /// <inheritdoc />
    public partial class SQLInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "42dd564e-b0b2-4a16-93ce-2f18e7b37444");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e101e06f-e826-49fa-ab81-978c70ec0ab3");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "3170084a-5c8b-4f56-94a3-3256fe1e9f54", "4de4a52c-0df1-495d-8977-4e3f25cbd164" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "3170084a-5c8b-4f56-94a3-3256fe1e9f54", "d1936bb7-a8ed-44fd-bef7-68da773b47c8" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "3170084a-5c8b-4f56-94a3-3256fe1e9f54", "e8c009e9-980a-49a2-8ec8-087b5b8735ce" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3170084a-5c8b-4f56-94a3-3256fe1e9f54");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4de4a52c-0df1-495d-8977-4e3f25cbd164");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d1936bb7-a8ed-44fd-bef7-68da773b47c8");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "e8c009e9-980a-49a2-8ec8-087b5b8735ce");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1069fead-3c2e-44d5-9bd1-be4d12e2fc1e", null, "Premium", "PREMIUM" },
                    { "521d237a-ce90-4d80-81a8-2ecc008c9b37", null, "Admin", "ADMIN" },
                    { "7866ffc8-4c6b-41af-8ebd-4649bc921419", null, "Basic", "BASIC" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Bday", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfileImage", "SecurityStamp", "Surname1", "Surname2", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "503c3a6a-b7d5-4083-abb2-bca2f7bdec5b", 0, new DateOnly(1968, 4, 5), "2e78bbc1-2e0b-4d58-ae78-1571e7e0947e", "cesarmatelat@gmail.com", true, false, null, "César Osvaldo", "CESARMATELAT@GMAIL.COM", "CESARMATELAT@GMAIL.COM", "AQAAAAIAAYagAAAAEFPC609Qr68j+0GZJb49pFVA0sSLkjzY3072fpjgnVloiDXM38H+iNiAclxm+GJEDw==", "664774821", false, "/imgs/profile/cesarmatelat@gmail.com/profile.jpg", "6fe03dd8-03d8-44fc-928c-8182fc2f4659", "Matelat", "Borneo", false, "cesarmatelat@gmail.com" },
                    { "9a8ebfd3-7e27-4c54-894e-6fd729ea1a70", 0, new DateOnly(2001, 1, 3), "687504fc-0188-469a-80f9-1e060d085d89", "patrickmurphygon@gmail.com", true, false, null, "Patrick Edward", "PATRICKMURPHYGON@GMAIL.COM", "PATRICKMURPHYGON@GMAIL.COM", "AQAAAAIAAYagAAAAEHBKkb7MjdjHzg+mQT99mmgz/teeHIhNLSkRvkMrzxVFC4rdnJkRACF3YXasMlMgiw==", "634547833", false, "/imgs/profile/patrickmurphygon@gmail.com/Patrick.jpg", "96f508bd-5f18-4da2-bb16-948dc49883c7", "Murphy", "González", false, "patrickmurphygon@gmail.com" },
                    { "9db23031-e530-4a2c-8cf2-c543155c7849", 0, new DateOnly(1995, 7, 15), "fd33f35d-1c98-412e-9a57-629ac567c5a4", "ledesma.leslie@gmail.com", true, false, null, "Leslie Ann", "LEDESMA.LESLIE@GMAIL.COM", "LEDESMA.LESLIE@GMAIL.COM", "AQAAAAIAAYagAAAAEMEvG9UvFRhYc1sLNcjdHnkDWURk4T/dYVwtwdbbtt5tKLnTez4zIsbIIq3TMLfZPA==", "644388160", false, "/imgs/profile/ledesma.leslie@gmail.com/Leslie.jpg", "1b9e335c-7bcc-429c-ba7b-6942262e8832", "Ledesma", "", false, "ledesma.leslie@gmail.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "521d237a-ce90-4d80-81a8-2ecc008c9b37", "503c3a6a-b7d5-4083-abb2-bca2f7bdec5b" },
                    { "521d237a-ce90-4d80-81a8-2ecc008c9b37", "9a8ebfd3-7e27-4c54-894e-6fd729ea1a70" },
                    { "521d237a-ce90-4d80-81a8-2ecc008c9b37", "9db23031-e530-4a2c-8cf2-c543155c7849" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1069fead-3c2e-44d5-9bd1-be4d12e2fc1e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7866ffc8-4c6b-41af-8ebd-4649bc921419");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "521d237a-ce90-4d80-81a8-2ecc008c9b37", "503c3a6a-b7d5-4083-abb2-bca2f7bdec5b" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "521d237a-ce90-4d80-81a8-2ecc008c9b37", "9a8ebfd3-7e27-4c54-894e-6fd729ea1a70" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "521d237a-ce90-4d80-81a8-2ecc008c9b37", "9db23031-e530-4a2c-8cf2-c543155c7849" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "521d237a-ce90-4d80-81a8-2ecc008c9b37");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "503c3a6a-b7d5-4083-abb2-bca2f7bdec5b");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9a8ebfd3-7e27-4c54-894e-6fd729ea1a70");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9db23031-e530-4a2c-8cf2-c543155c7849");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3170084a-5c8b-4f56-94a3-3256fe1e9f54", null, "Admin", "ADMIN" },
                    { "42dd564e-b0b2-4a16-93ce-2f18e7b37444", null, "Basic", "BASIC" },
                    { "e101e06f-e826-49fa-ab81-978c70ec0ab3", null, "Premium", "PREMIUM" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Bday", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfileImage", "SecurityStamp", "Surname1", "Surname2", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "4de4a52c-0df1-495d-8977-4e3f25cbd164", 0, new DateOnly(1968, 4, 5), "32b20034-ea04-4439-90be-7a4c6b89da75", "cesarmatelat@gmail.com", true, false, null, "César Osvaldo", "CESARMATELAT@GMAIL.COM", "CESARMATELAT@GMAIL.COM", "AQAAAAIAAYagAAAAEMfMPNDrB80M3iIrPK+uxmvZG/9AnTJA6FM5nDptAN5QsxW2Ou6s76a7PlVgfGFpzg==", "664774821", false, "/imgs/profile/cesarmatelat@gmail.com/profile.jpg", "c8d13bc7-1559-4a0d-864f-a6f0bff77579", "Matelat", "Borneo", false, "cesarmatelat@gmail.com" },
                    { "d1936bb7-a8ed-44fd-bef7-68da773b47c8", 0, new DateOnly(1995, 7, 15), "6db0368e-e7d8-4c3f-be00-d803da357a64", "ledesma.leslie@gmail.com", true, false, null, "Leslie Ann", "LEDESMA.LESLIE@GMAIL.COM", "LEDESMA.LESLIE@GMAIL.COM", "AQAAAAIAAYagAAAAEFWbSYw0z0xf1IjINyadIC5vWpovSOu2vsqBkI8u1yEtUSk2SchYFrYNuRg7rcfoog==", "644388160", false, "/imgs/profile/ledesma.leslie@gmail.com/Leslie.jpg", "05258f38-9483-49e8-a95c-d982534bc3ad", "Ledesma", "", false, "ledesma.leslie@gmail.com" },
                    { "e8c009e9-980a-49a2-8ec8-087b5b8735ce", 0, new DateOnly(2001, 1, 3), "f6c54ee8-e706-4ac5-b64c-88c932acd286", "patrickmurphygon@gmail.com", true, false, null, "Patrick Edward", "PATRICKMURPHYGON@GMAIL.COM", "PATRICKMURPHYGON@GMAIL.COM", "AQAAAAIAAYagAAAAEIr+Hq/VfBXQLe6yF9sKup/ElHEYFl3eoR0Jt12ZKSnCJMqdHlvKeXrtJPtAMjOVZQ==", "634547833", false, "/imgs/profile/patrickmurphygon@gmail.com/Patrick.jpg", "1227ac7b-423b-4c36-9837-95e549384689", "Murphy", "González", false, "patrickmurphygon@gmail.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "3170084a-5c8b-4f56-94a3-3256fe1e9f54", "4de4a52c-0df1-495d-8977-4e3f25cbd164" },
                    { "3170084a-5c8b-4f56-94a3-3256fe1e9f54", "d1936bb7-a8ed-44fd-bef7-68da773b47c8" },
                    { "3170084a-5c8b-4f56-94a3-3256fe1e9f54", "e8c009e9-980a-49a2-8ec8-087b5b8735ce" }
                });
        }
    }
}
