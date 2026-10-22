using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelpDeskUnev.Migrations
{
    /// <inheritdoc />
    public partial class FixTicketRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Usuarios_TecnicoIdUsuario",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_TecnicoIdUsuario",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "Especialidad",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "TicketsAsignados",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "TecnicoIdUsuario",
                table: "Tickets");

            migrationBuilder.RenameColumn(
                name: "Estado",
                table: "Usuarios",
                newName: "EsActivo");

            migrationBuilder.AddColumn<string>(
                name: "AreaEspecialidad",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Titulo",
                table: "Tickets",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AreaEspecialidad",
                table: "Usuarios");

            migrationBuilder.RenameColumn(
                name: "EsActivo",
                table: "Usuarios",
                newName: "Estado");

            migrationBuilder.AddColumn<string>(
                name: "Especialidad",
                table: "Usuarios",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TicketsAsignados",
                table: "Usuarios",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Titulo",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AddColumn<int>(
                name: "TecnicoIdUsuario",
                table: "Tickets",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_TecnicoIdUsuario",
                table: "Tickets",
                column: "TecnicoIdUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Usuarios_TecnicoIdUsuario",
                table: "Tickets",
                column: "TecnicoIdUsuario",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
