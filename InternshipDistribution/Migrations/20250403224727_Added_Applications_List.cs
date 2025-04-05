using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternshipDistribution.Migrations
{
    /// <inheritdoc />
    public partial class Added_Applications_List : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DistributionApplication_Companies_Priority1CompanyId",
                table: "DistributionApplication");

            migrationBuilder.DropForeignKey(
                name: "FK_DistributionApplication_Companies_Priority2CompanyId",
                table: "DistributionApplication");

            migrationBuilder.DropForeignKey(
                name: "FK_DistributionApplication_Companies_Priority3CompanyId",
                table: "DistributionApplication");

            migrationBuilder.DropForeignKey(
                name: "FK_DistributionApplication_Students_StudentId",
                table: "DistributionApplication");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DistributionApplication",
                table: "DistributionApplication");

            migrationBuilder.RenameTable(
                name: "DistributionApplication",
                newName: "DistributionApplications");

            migrationBuilder.RenameIndex(
                name: "IX_DistributionApplication_StudentId",
                table: "DistributionApplications",
                newName: "IX_DistributionApplications_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_DistributionApplication_Priority3CompanyId",
                table: "DistributionApplications",
                newName: "IX_DistributionApplications_Priority3CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_DistributionApplication_Priority2CompanyId",
                table: "DistributionApplications",
                newName: "IX_DistributionApplications_Priority2CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_DistributionApplication_Priority1CompanyId",
                table: "DistributionApplications",
                newName: "IX_DistributionApplications_Priority1CompanyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DistributionApplications",
                table: "DistributionApplications",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DistributionApplications_Companies_Priority1CompanyId",
                table: "DistributionApplications",
                column: "Priority1CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DistributionApplications_Companies_Priority2CompanyId",
                table: "DistributionApplications",
                column: "Priority2CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DistributionApplications_Companies_Priority3CompanyId",
                table: "DistributionApplications",
                column: "Priority3CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DistributionApplications_Students_StudentId",
                table: "DistributionApplications",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DistributionApplications_Companies_Priority1CompanyId",
                table: "DistributionApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_DistributionApplications_Companies_Priority2CompanyId",
                table: "DistributionApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_DistributionApplications_Companies_Priority3CompanyId",
                table: "DistributionApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_DistributionApplications_Students_StudentId",
                table: "DistributionApplications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DistributionApplications",
                table: "DistributionApplications");

            migrationBuilder.RenameTable(
                name: "DistributionApplications",
                newName: "DistributionApplication");

            migrationBuilder.RenameIndex(
                name: "IX_DistributionApplications_StudentId",
                table: "DistributionApplication",
                newName: "IX_DistributionApplication_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_DistributionApplications_Priority3CompanyId",
                table: "DistributionApplication",
                newName: "IX_DistributionApplication_Priority3CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_DistributionApplications_Priority2CompanyId",
                table: "DistributionApplication",
                newName: "IX_DistributionApplication_Priority2CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_DistributionApplications_Priority1CompanyId",
                table: "DistributionApplication",
                newName: "IX_DistributionApplication_Priority1CompanyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DistributionApplication",
                table: "DistributionApplication",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DistributionApplication_Companies_Priority1CompanyId",
                table: "DistributionApplication",
                column: "Priority1CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DistributionApplication_Companies_Priority2CompanyId",
                table: "DistributionApplication",
                column: "Priority2CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DistributionApplication_Companies_Priority3CompanyId",
                table: "DistributionApplication",
                column: "Priority3CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DistributionApplication_Students_StudentId",
                table: "DistributionApplication",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
