using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoMVC.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTableAgentToAgency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AgentName",
                table: "DistributionSystems",
                newName: "AgencyName");

            migrationBuilder.RenameColumn(
                name: "AgentId",
                table: "DistributionSystems",
                newName: "AgencyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AgencyName",
                table: "DistributionSystems",
                newName: "AgentName");

            migrationBuilder.RenameColumn(
                name: "AgencyId",
                table: "DistributionSystems",
                newName: "AgentId");
        }
    }
}
