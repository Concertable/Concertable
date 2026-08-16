using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Concertable.B2B.Deal.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "deal");

            migrationBuilder.CreateTable(
                name: "DealTerms",
                schema: "deal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealTerms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DoorSplitTerms",
                schema: "deal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ArtistDoorPercent = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoorSplitTerms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoorSplitTerms_DealTerms_Id",
                        column: x => x.Id,
                        principalSchema: "deal",
                        principalTable: "DealTerms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlatFeeTerms",
                schema: "deal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Fee = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlatFeeTerms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlatFeeTerms_DealTerms_Id",
                        column: x => x.Id,
                        principalSchema: "deal",
                        principalTable: "DealTerms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VenueHireTerms",
                schema: "deal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    HireFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VenueHireTerms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VenueHireTerms_DealTerms_Id",
                        column: x => x.Id,
                        principalSchema: "deal",
                        principalTable: "DealTerms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VersusTerms",
                schema: "deal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Guarantee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ArtistDoorPercent = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VersusTerms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VersusTerms_DealTerms_Id",
                        column: x => x.Id,
                        principalSchema: "deal",
                        principalTable: "DealTerms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DoorSplitTerms",
                schema: "deal");

            migrationBuilder.DropTable(
                name: "FlatFeeTerms",
                schema: "deal");

            migrationBuilder.DropTable(
                name: "VenueHireTerms",
                schema: "deal");

            migrationBuilder.DropTable(
                name: "VersusTerms",
                schema: "deal");

            migrationBuilder.DropTable(
                name: "DealTerms",
                schema: "deal");
        }
    }
}
