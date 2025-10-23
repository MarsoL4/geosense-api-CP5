using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeoSense.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCleanSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PATIO",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOME = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PATIO", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "USUARIO",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOME = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    EMAIL = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    SENHA = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false),
                    TIPO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USUARIO", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "VAGA",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NUMERO = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    TIPO = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    STATUS = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    PATIO_ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VAGA", x => x.ID);
                    table.ForeignKey(
                        name: "FK_VAGA_PATIO_PATIO_ID",
                        column: x => x.PATIO_ID,
                        principalTable: "PATIO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MOTO",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    MODELO = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    PLACA = table.Column<string>(type: "NVARCHAR2(10)", maxLength: 10, nullable: false),
                    CHASSI = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    PROBLEMA_IDENTIFICADO = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: true),
                    VAGA_ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MOTO", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MOTO_VAGA_VAGA_ID",
                        column: x => x.VAGA_ID,
                        principalTable: "VAGA",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ALOCACAOMOTO",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    DATA_HORA_ALOCACAO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    MOTO_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    VAGA_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
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
                    DESCRICAO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    TIPOS_DEFEITOS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    MOTO_ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_MOTO_CHASSI",
                table: "MOTO",
                column: "CHASSI",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MOTO_PLACA",
                table: "MOTO",
                column: "PLACA",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MOTO_VAGA_ID",
                table: "MOTO",
                column: "VAGA_ID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_USUARIO_EMAIL",
                table: "USUARIO",
                column: "EMAIL",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VAGA_NUMERO_PATIO_ID",
                table: "VAGA",
                columns: new[] { "NUMERO", "PATIO_ID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VAGA_PATIO_ID",
                table: "VAGA",
                column: "PATIO_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ALOCACAOMOTO");

            migrationBuilder.DropTable(
                name: "DEFEITO");

            migrationBuilder.DropTable(
                name: "USUARIO");

            migrationBuilder.DropTable(
                name: "MOTO");

            migrationBuilder.DropTable(
                name: "VAGA");

            migrationBuilder.DropTable(
                name: "PATIO");
        }
    }
}
