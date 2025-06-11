using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixaddingNotify : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClienteId",
                table: "Notificacoes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notificacoes_ClienteId",
                table: "Notificacoes",
                column: "ClienteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notificacoes_Clientes_ClienteId",
                table: "Notificacoes",
                column: "ClienteId",
                principalTable: "Clientes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notificacoes_Clientes_ClienteId",
                table: "Notificacoes");

            migrationBuilder.DropIndex(
                name: "IX_Notificacoes_ClienteId",
                table: "Notificacoes");

            migrationBuilder.DropColumn(
                name: "ClienteId",
                table: "Notificacoes");
        }
    }
}
