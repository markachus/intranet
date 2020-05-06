using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IntranetCore.Data.Migrations
{
    public partial class InitialCore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Entradas",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UsuarioCreacion = table.Column<string>(maxLength: 50, nullable: false),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    UsuarioModificacion = table.Column<string>(maxLength: 50, nullable: false),
                    FechaUltimaModificacion = table.Column<DateTime>(nullable: false),
                    Eliminado = table.Column<bool>(nullable: false),
                    Titulo = table.Column<string>(maxLength: 100, nullable: false),
                    Contenido = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entradas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Etiquetas",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UsuarioCreacion = table.Column<string>(maxLength: 50, nullable: false),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    UsuarioModificacion = table.Column<string>(maxLength: 50, nullable: false),
                    FechaUltimaModificacion = table.Column<DateTime>(nullable: false),
                    Eliminado = table.Column<bool>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 50, nullable: false),
                    HexColor = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Etiquetas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EntradaEtiqueta",
                columns: table => new
                {
                    EntradaId = table.Column<Guid>(nullable: false),
                    EtiquetaId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntradaEtiqueta", x => new { x.EntradaId, x.EtiquetaId });
                    table.ForeignKey(
                        name: "FK_EntradaEtiqueta_Entradas_EntradaId",
                        column: x => x.EntradaId,
                        principalTable: "Entradas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntradaEtiqueta_Etiquetas_EtiquetaId",
                        column: x => x.EtiquetaId,
                        principalTable: "Etiquetas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Entradas",
                columns: new[] { "Id", "Contenido", "Eliminado", "FechaCreacion", "FechaUltimaModificacion", "Titulo", "UsuarioCreacion", "UsuarioModificacion" },
                values: new object[,]
                {
                    { new Guid("cf7dc085-62a8-492d-8f60-509bcc836d02"), "Adjuntar correos en Gmail es muy sencillo", false, new DateTime(2020, 5, 6, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2020, 5, 6, 16, 56, 56, 668, DateTimeKind.Local).AddTicks(3098), "Adjuntar correos a correos en Gmail", "Admin", "Admin" },
                    { new Guid("36232aa4-81a1-4962-b732-06d3204113ee"), "La empresa ha solicitado un ERTE con fecha efectiva 1 de Abril y esta esperando respuesta.", false, new DateTime(2020, 5, 6, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2020, 5, 6, 16, 56, 56, 668, DateTimeKind.Local).AddTicks(6045), "Solictud de ERTE por Covid-19", "Admin", "Admin" },
                    { new Guid("98f1191f-beeb-4e35-a1fe-c7f5bccec64f"), "Disponéis de 22 días laborables. Teneis el calendario laboral 2020 aquí: [link]", false, new DateTime(2020, 5, 6, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2020, 5, 6, 16, 56, 56, 668, DateTimeKind.Local).AddTicks(6208), "Vacaciones 2020", "Admin", "Admin" },
                    { new Guid("c7ddb8bb-4ee3-4443-b7ab-33aefc176c3b"), "A continuación os indicamos la dirección de acceso al PMS de cada Hotel", false, new DateTime(2020, 5, 6, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2020, 5, 6, 16, 56, 56, 668, DateTimeKind.Local).AddTicks(6270), "Dirección web de PMS de cada Hotel", "Admin", "Admin" },
                    { new Guid("011499a9-911d-4ef0-8929-6a055f28acb5"), "Las fecha para 2020 son las siguientes. Además el congreso se ha prolongado un año más debido a la cancelación en 2019", false, new DateTime(2020, 5, 6, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2020, 5, 6, 16, 56, 56, 668, DateTimeKind.Local).AddTicks(6322), "Fechas MWC 2020", "Admin", "Admin" }
                });

            migrationBuilder.InsertData(
                table: "Etiquetas",
                columns: new[] { "Id", "Eliminado", "FechaCreacion", "FechaUltimaModificacion", "HexColor", "Nombre", "UsuarioCreacion", "UsuarioModificacion" },
                values: new object[,]
                {
                    { new Guid("766c0959-47bc-47a6-a4f2-6c0a5cea7162"), false, new DateTime(2020, 5, 6, 16, 56, 56, 663, DateTimeKind.Local).AddTicks(9240), new DateTime(2020, 5, 6, 16, 56, 56, 666, DateTimeKind.Local).AddTicks(5378), null, "Prestige", "Admin", "Admin" },
                    { new Guid("75e97d33-8d01-46cb-8a6c-3e1bc062a29a"), false, new DateTime(2020, 5, 6, 16, 56, 56, 666, DateTimeKind.Local).AddTicks(6629), new DateTime(2020, 5, 6, 16, 56, 56, 666, DateTimeKind.Local).AddTicks(6666), null, "Correo", "Admin", "Admin" },
                    { new Guid("dab328aa-1f07-423f-a5bf-8ffa1cc1c0a8"), false, new DateTime(2020, 5, 6, 16, 56, 56, 666, DateTimeKind.Local).AddTicks(6695), new DateTime(2020, 5, 6, 16, 56, 56, 666, DateTimeKind.Local).AddTicks(6699), null, "Seguridad", "Admin", "Admin" },
                    { new Guid("98c8e0f6-34c2-46a4-b2a8-f1ee855e9493"), false, new DateTime(2020, 5, 6, 16, 56, 56, 666, DateTimeKind.Local).AddTicks(6705), new DateTime(2020, 5, 6, 16, 56, 56, 666, DateTimeKind.Local).AddTicks(6708), null, "Personal", "Admin", "Admin" },
                    { new Guid("9d49c27b-2c4d-4c0e-8e09-d9f51e2f2cdb"), false, new DateTime(2020, 5, 6, 16, 56, 56, 666, DateTimeKind.Local).AddTicks(6715), new DateTime(2020, 5, 6, 16, 56, 56, 666, DateTimeKind.Local).AddTicks(6718), null, "General", "Admin", "Admin" },
                    { new Guid("a182f6bc-c2f4-442e-830b-ae5c6810f517"), false, new DateTime(2020, 5, 6, 16, 56, 56, 666, DateTimeKind.Local).AddTicks(6724), new DateTime(2020, 5, 6, 16, 56, 56, 666, DateTimeKind.Local).AddTicks(6727), null, "MWC", "Admin", "Admin" }
                });

            migrationBuilder.InsertData(
                table: "EntradaEtiqueta",
                columns: new[] { "EntradaId", "EtiquetaId" },
                values: new object[,]
                {
                    { new Guid("c7ddb8bb-4ee3-4443-b7ab-33aefc176c3b"), new Guid("766c0959-47bc-47a6-a4f2-6c0a5cea7162") },
                    { new Guid("011499a9-911d-4ef0-8929-6a055f28acb5"), new Guid("766c0959-47bc-47a6-a4f2-6c0a5cea7162") },
                    { new Guid("cf7dc085-62a8-492d-8f60-509bcc836d02"), new Guid("75e97d33-8d01-46cb-8a6c-3e1bc062a29a") },
                    { new Guid("36232aa4-81a1-4962-b732-06d3204113ee"), new Guid("98c8e0f6-34c2-46a4-b2a8-f1ee855e9493") },
                    { new Guid("98f1191f-beeb-4e35-a1fe-c7f5bccec64f"), new Guid("98c8e0f6-34c2-46a4-b2a8-f1ee855e9493") },
                    { new Guid("36232aa4-81a1-4962-b732-06d3204113ee"), new Guid("9d49c27b-2c4d-4c0e-8e09-d9f51e2f2cdb") },
                    { new Guid("98f1191f-beeb-4e35-a1fe-c7f5bccec64f"), new Guid("9d49c27b-2c4d-4c0e-8e09-d9f51e2f2cdb") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_EntradaEtiqueta_EtiquetaId",
                table: "EntradaEtiqueta",
                column: "EtiquetaId");

            migrationBuilder.CreateIndex(
                name: "IX_Etiquetas_Nombre",
                table: "Etiquetas",
                column: "Nombre",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntradaEtiqueta");

            migrationBuilder.DropTable(
                name: "Entradas");

            migrationBuilder.DropTable(
                name: "Etiquetas");
        }
    }
}
