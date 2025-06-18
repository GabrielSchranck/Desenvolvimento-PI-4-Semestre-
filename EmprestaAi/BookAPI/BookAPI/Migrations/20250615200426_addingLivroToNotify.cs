using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookAPI.Migrations
{
    /// <inheritdoc />
    public partial class addingLivroToNotify : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LivroId",
                table: "Notificacoes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notificacoes_LivroId",
                table: "Notificacoes",
                column: "LivroId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notificacoes_Livros_LivroId",
                table: "Notificacoes",
                column: "LivroId",
                principalTable: "Livros",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notificacoes_Livros_LivroId",
                table: "Notificacoes");

            migrationBuilder.DropIndex(
                name: "IX_Notificacoes_LivroId",
                table: "Notificacoes");

            migrationBuilder.DropColumn(
                name: "LivroId",
                table: "Notificacoes");
        }
    }
}
