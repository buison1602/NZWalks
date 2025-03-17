using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NZWalks.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataForDifficyltiesandRegions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Difficulties",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("62a957aa-a3f3-4e18-bfef-211bd8696da9"), "Hard" },
                    { new Guid("ad8182f8-b442-4ea4-9fe0-dc95f96455a7"), "Easy" },
                    { new Guid("afa9882b-36da-44e5-965d-9196841547ae"), "Medium" }
                });

            migrationBuilder.InsertData(
                table: "Regions",
                columns: new[] { "Id", "Code", "Name", "RegionImageUrl" },
                values: new object[,]
                {
                    { new Guid("29219b8f-d095-4241-a0a7-b1bb7c233a5b"), "AUK 4", "Auckland 4", "https://www.doc.govt.nz/globalassets/images/conservation/parks-and-recreation/places-to-visit/auckland/auckland-region.jpg" },
                    { new Guid("302cd4ff-5732-411d-82de-cf1f208379fb"), "AUK 1", "Auckland 1", "https://www.doc.govt.nz/globalassets/images/conservation/parks-and-recreation/places-to-visit/auckland/auckland-region.jpg" },
                    { new Guid("302cd4ff-5732-411d-82de-cf1f208379fc"), "AUK 2", "Auckland 2", "https://www.doc.govt.nz/globalassets/images/conservation/parks-and-recreation/places-to-visit/auckland/auckland-region.jpg" },
                    { new Guid("5ca0dd19-f32c-4921-9908-57c64c057a1d"), "AUK 3", "Auckland 3", "https://www.doc.govt.nz/globalassets/images/conservation/parks-and-recreation/places-to-visit/auckland/auckland-region.jpg" },
                    { new Guid("d21e272f-a127-4ab1-a8b0-c03d61f10035"), "AUK 5", "Auckland 5", "https://www.doc.govt.nz/globalassets/images/conservation/parks-and-recreation/places-to-visit/auckland/auckland-region.jpg" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("62a957aa-a3f3-4e18-bfef-211bd8696da9"));

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("ad8182f8-b442-4ea4-9fe0-dc95f96455a7"));

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("afa9882b-36da-44e5-965d-9196841547ae"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("29219b8f-d095-4241-a0a7-b1bb7c233a5b"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("302cd4ff-5732-411d-82de-cf1f208379fb"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("302cd4ff-5732-411d-82de-cf1f208379fc"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("5ca0dd19-f32c-4921-9908-57c64c057a1d"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("d21e272f-a127-4ab1-a8b0-c03d61f10035"));
        }
    }
}
