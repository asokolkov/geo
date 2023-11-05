using System;
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
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    iso3116_alpha2 = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    iso3166_alpha3 = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    phone_code = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    phone_mask = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    osm = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    last_updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_countries", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "translation_languages",
                columns: table => new
                {
                    language = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_translation_languages", x => x.language);
                });

            migrationBuilder.CreateTable(
                name: "translations",
                columns: table => new
                {
                    entity_id = table.Column<int>(type: "integer", nullable: false),
                    type = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    language_iso639 = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    translation = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    last_updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_translations", x => new { x.type, x.entity_id, x.language_iso639 });
                });

            migrationBuilder.CreateTable(
                name: "regions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    country_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    osm = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    last_updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_regions", x => x.id);
                    table.ForeignKey(
                        name: "FK_regions_countries_country_id",
                        column: x => x.country_id,
                        principalTable: "countries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cities",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CountryId = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    latitude = table.Column<double>(type: "double precision", nullable: false),
                    longitude = table.Column<double>(type: "double precision", nullable: false),
                    timezone = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    osm = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    last_updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    RegionId = table.Column<int>(type: "integer", nullable: false),
                    iata = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
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
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    iata_en = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    latitude = table.Column<double>(type: "double precision", nullable: false),
                    longitude = table.Column<double>(type: "double precision", nullable: false),
                    timezone = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    osm = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    last_updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    iata_ru = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
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
                    rzd_code = table.Column<int>(type: "integer", nullable: false),
                    is_main = table.Column<bool>(type: "boolean", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    latitude = table.Column<double>(type: "double precision", nullable: false),
                    longitude = table.Column<double>(type: "double precision", nullable: false),
                    timezone = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "IX_airports_CityId",
                table: "airports",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_airports_iata_en",
                table: "airports",
                column: "iata_en",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_airports_iata_ru",
                table: "airports",
                column: "iata_ru",
                unique: true);

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
                name: "IX_railway_stations_city_id",
                table: "railway_stations",
                column: "city_id");

            migrationBuilder.CreateIndex(
                name: "IX_railway_stations_osm",
                table: "railway_stations",
                column: "osm",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_railway_stations_rzd_code",
                table: "railway_stations",
                column: "rzd_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_regions_country_id",
                table: "regions",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "IX_translations_translation",
                table: "translations",
                column: "translation");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "airports");

            migrationBuilder.DropTable(
                name: "railway_stations");

            migrationBuilder.DropTable(
                name: "translation_languages");

            migrationBuilder.DropTable(
                name: "translations");

            migrationBuilder.DropTable(
                name: "cities");

            migrationBuilder.DropTable(
                name: "regions");

            migrationBuilder.DropTable(
                name: "countries");
        }
    }
}
