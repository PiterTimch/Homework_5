using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerNovaPost.Migrations
{
    /// <inheritdoc />
    public partial class tbl_areas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_areas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Ref = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    AreasCenter = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_areas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Ref = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    TypeDescription = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    AreaEntityId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_cities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbl_cities_tbl_areas_AreaEntityId",
                        column: x => x.AreaEntityId,
                        principalTable: "tbl_areas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "tbl_departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Ref = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Address = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    CityEntityId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbl_departments_tbl_cities_CityEntityId",
                        column: x => x.CityEntityId,
                        principalTable: "tbl_cities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_cities_AreaEntityId",
                table: "tbl_cities",
                column: "AreaEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_departments_CityEntityId",
                table: "tbl_departments",
                column: "CityEntityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_departments");

            migrationBuilder.DropTable(
                name: "tbl_cities");

            migrationBuilder.DropTable(
                name: "tbl_areas");
        }
    }
}
