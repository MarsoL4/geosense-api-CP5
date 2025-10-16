using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeoSense.API.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAlocacaoMotoeDefeito : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ALOCACAOMOTO");

            migrationBuilder.DropTable(
                name: "DEFEITO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ALOCACAOMOTO",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    MOTO_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    VAGA_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    DATA_HORA_ALOCACAO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    MECANICO_RESPONSAVEL_ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ALOCACAOMOTO", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ALOCACAOMOTO_MOTO_MOTO_ID",
                        column: x => x.MOTO_ID,
                        principalTable: "MOTO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ALOCACAOMOTO_VAGA_VAGA_ID",
                        column: x => x.VAGA_ID,
                        principalTable: "VAGA",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DEFEITO",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    MOTO_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    DESCRICAO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    TIPOS_DEFEITOS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DEFEITO", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DEFEITO_MOTO_MOTO_ID",
                        column: x => x.MOTO_ID,
                        principalTable: "MOTO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ALOCACAOMOTO_MOTO_ID",
                table: "ALOCACAOMOTO",
                column: "MOTO_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ALOCACAOMOTO_VAGA_ID",
                table: "ALOCACAOMOTO",
                column: "VAGA_ID");

            migrationBuilder.CreateIndex(
                name: "IX_DEFEITO_MOTO_ID",
                table: "DEFEITO",
                column: "MOTO_ID");
        }
    }
}
