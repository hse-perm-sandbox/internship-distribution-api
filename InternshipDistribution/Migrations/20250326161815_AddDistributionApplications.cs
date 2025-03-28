using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace InternshipDistribution.Migrations
{
    /// <inheritdoc />
    public partial class AddDistributionApplications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DistributionApplication",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StudentId = table.Column<int>(type: "integer", nullable: false),
                    Priority1CompanyId = table.Column<int>(type: "integer", nullable: true),
                    Priority2CompanyId = table.Column<int>(type: "integer", nullable: true),
                    Priority3CompanyId = table.Column<int>(type: "integer", nullable: true),
                    Priority1Status = table.Column<int>(type: "integer", nullable: false),
                    Priority2Status = table.Column<int>(type: "integer", nullable: false),
                    Priority3Status = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DistributionApplication", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DistributionApplication_Companies_Priority1CompanyId",
                        column: x => x.Priority1CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DistributionApplication_Companies_Priority2CompanyId",
                        column: x => x.Priority2CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DistributionApplication_Companies_Priority3CompanyId",
                        column: x => x.Priority3CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DistributionApplication_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DistributionApplication_Priority1CompanyId",
                table: "DistributionApplication",
                column: "Priority1CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_DistributionApplication_Priority2CompanyId",
                table: "DistributionApplication",
                column: "Priority2CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_DistributionApplication_Priority3CompanyId",
                table: "DistributionApplication",
                column: "Priority3CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_DistributionApplication_StudentId",
                table: "DistributionApplication",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DistributionApplication");
        }
    }
}
