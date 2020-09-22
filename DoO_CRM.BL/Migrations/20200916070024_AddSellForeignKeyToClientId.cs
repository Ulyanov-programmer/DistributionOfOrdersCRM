using Microsoft.EntityFrameworkCore.Migrations;

namespace DoO_CRM.BL.Migrations
{
    public partial class AddSellForeignKeyToClientId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "Sells",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sells_ClientId",
                table: "Sells",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sells_Clients_ClientId",
                table: "Sells",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sells_Clients_ClientId",
                table: "Sells");

            migrationBuilder.DropIndex(
                name: "IX_Sells_ClientId",
                table: "Sells");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Sells");
        }
    }
}
