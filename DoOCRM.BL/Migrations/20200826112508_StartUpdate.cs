using Microsoft.EntityFrameworkCore.Migrations;

namespace DoO_CRM.BL.Migrations
{
    public partial class StartUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sells_Orders_OrderId1",
                table: "Sells");

            migrationBuilder.DropIndex(
                name: "IX_Sells_OrderId1",
                table: "Sells");

            migrationBuilder.DropColumn(
                name: "OrderId1",
                table: "Sells");

            migrationBuilder.AlterColumn<long>(
                name: "OrderId",
                table: "Sells",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Sells_OrderId",
                table: "Sells",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sells_Orders_OrderId",
                table: "Sells",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sells_Orders_OrderId",
                table: "Sells");

            migrationBuilder.DropIndex(
                name: "IX_Sells_OrderId",
                table: "Sells");

            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "Sells",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "OrderId1",
                table: "Sells",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sells_OrderId1",
                table: "Sells",
                column: "OrderId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Sells_Orders_OrderId1",
                table: "Sells",
                column: "OrderId1",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
