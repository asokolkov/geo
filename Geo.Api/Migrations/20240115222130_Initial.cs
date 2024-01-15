using System;
using Geo.Api.Repositories.Airports.Models;
using Geo.Api.Repositories.Cities.Models;
using Geo.Api.Repositories.Countries.Models;
using Geo.Api.Repositories.RailwayStations.Models;
using Geo.Api.Repositories.Regions.Models;
using Geo.Api.Repositories.SubwayStations.Models;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Geo.Api.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "countries",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<CountryNameEntity>(type: "jsonb", nullable: false),
                    iso3116_alpha2 = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    iso3166_alpha3 = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    phone_code = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    phone_mask = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    osm = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    NeedAutomaticUpdate = table.Column<bool>(type: "boolean", nullable: false),
                    last_updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    geometry = table.Column<CountryGeometryEntity>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_countries", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "regions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    country_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<RegionNameEntity>(type: "jsonb", nullable: false),
                    osm = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    NeedToUpdate = table.Column<bool>(type: "boolean", nullable: false),
                    last_updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    geometry = table.Column<RegionGeometryEntity>(type: "jsonb", nullable: false),
                    utc_offset = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_regions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "cities",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    iata = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    CountryId = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<CityNameEntity>(type: "jsonb", nullable: false),
                    geometry = table.Column<CityGeometryEntity>(type: "jsonb", nullable: false),
                    osm = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    need_to_update = table.Column<bool>(type: "boolean", nullable: false),
                    last_updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    RegionId = table.Column<int>(type: "integer", nullable: true),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    utc_offset = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cities", x => x.id);
                    table.ForeignKey(
                        name: "FK_cities_countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "countries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_cities_regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "regions",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "airports",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CityId = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<AirportNameEntity>(type: "jsonb", nullable: false),
                    code = table.Column<AirportCodeEntity>(type: "jsonb", nullable: false),
                    geometry = table.Column<AirportGeometryEntity>(type: "jsonb", nullable: false),
                    utc_offset = table.Column<int>(type: "integer", nullable: true),
                    osm = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    last_updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    need_automatic_update = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_airports", x => x.id);
                    table.ForeignKey(
                        name: "FK_airports_cities_CityId",
                        column: x => x.CityId,
                        principalTable: "cities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "railway_stations",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    city_id = table.Column<int>(type: "integer", nullable: false),
                    code = table.Column<RailwayStationCodeEntity>(type: "jsonb", nullable: false),
                    name = table.Column<RailwayStationNameEntity>(type: "jsonb", nullable: false),
                    geometry = table.Column<RailwayStationGeometryEntity>(type: "jsonb", nullable: false),
                    utc_offset = table.Column<int>(type: "integer", nullable: true),
                    need_to_update = table.Column<bool>(type: "boolean", nullable: false),
                    osm = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    last_updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_railway_stations", x => x.id);
                    table.ForeignKey(
                        name: "FK_railway_stations_cities_city_id",
                        column: x => x.city_id,
                        principalTable: "cities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "subway_stations",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    station_name = table.Column<SubwayStationNameEntity>(type: "jsonb", nullable: false),
                    line_name = table.Column<SubwayLineNameEntity>(type: "jsonb", nullable: false),
                    geometry = table.Column<SubwayStationGeometryEntity>(type: "jsonb", nullable: false),
                    CityId = table.Column<int>(type: "integer", nullable: false),
                    osm = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    need_automatic_update = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    last_updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subway_stations", x => x.id);
                    table.ForeignKey(
                        name: "FK_subway_stations_cities_CityId",
                        column: x => x.CityId,
                        principalTable: "cities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_airports_CityId",
                table: "airports",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_airports_osm",
                table: "airports",
                column: "osm",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_cities_CountryId",
                table: "cities",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_cities_iata",
                table: "cities",
                column: "iata",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_cities_osm",
                table: "cities",
                column: "osm",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_cities_RegionId",
                table: "cities",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_countries_iso3116_alpha2",
                table: "countries",
                column: "iso3116_alpha2",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_countries_iso3166_alpha3",
                table: "countries",
                column: "iso3166_alpha3",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_countries_osm",
                table: "countries",
                column: "osm");

            migrationBuilder.CreateIndex(
                name: "IX_railway_stations_city_id",
                table: "railway_stations",
                column: "city_id");

            migrationBuilder.CreateIndex(
                name: "IX_railway_stations_osm",
                table: "railway_stations",
                column: "osm",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_regions_country_id",
                table: "regions",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "IX_subway_stations_CityId",
                table: "subway_stations",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_subway_stations_osm",
                table: "subway_stations",
                column: "osm",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "airports");

            migrationBuilder.DropTable(
                name: "railway_stations");

            migrationBuilder.DropTable(
                name: "subway_stations");

            migrationBuilder.DropTable(
                name: "cities");

            migrationBuilder.DropTable(
                name: "countries");

            migrationBuilder.DropTable(
                name: "regions");
        }
    }
}
