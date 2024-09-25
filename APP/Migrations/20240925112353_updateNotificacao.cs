using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeData_Connection.Migrations
{
    /// <inheritdoc />
    public partial class updateNotificacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notificacao_AspNetUsers_UserID",
                table: "Notificacao");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notificacao",
                table: "Notificacao");

            migrationBuilder.RenameTable(
                name: "Notificacao",
                newName: "Notificacoes");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Notificacoes",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Notificacao_UserID",
                table: "Notificacoes",
                newName: "IX_Notificacoes_UserId");

            migrationBuilder.AddColumn<string>(
                name: "UserID",
                table: "Notificacoes",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notificacoes",
                table: "Notificacoes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notificacoes_AspNetUsers_UserId",
                table: "Notificacoes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notificacoes_AspNetUsers_UserId",
                table: "Notificacoes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notificacoes",
                table: "Notificacoes");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Notificacoes");

            migrationBuilder.RenameTable(
                name: "Notificacoes",
                newName: "Notificacao");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Notificacao",
                newName: "UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Notificacoes_UserId",
                table: "Notificacao",
                newName: "IX_Notificacao_UserID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notificacao",
                table: "Notificacao",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notificacao_AspNetUsers_UserID",
                table: "Notificacao",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
