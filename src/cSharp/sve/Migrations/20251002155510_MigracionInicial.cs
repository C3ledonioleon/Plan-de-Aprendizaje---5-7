using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sve_api.Migrations
{
    /// <inheritdoc />
    public partial class MigracionInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Evento",
                columns: table => new
                {
                    IdEvento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descripcion = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaInicio = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evento", x => x.IdEvento);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Local",
                columns: table => new
                {
                    IdLocal = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Direccion = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CapacidadTotal = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Local", x => x.IdLocal);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Apodo = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    contrasenia = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Rol = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.IdUsuario);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Funcion",
                columns: table => new
                {
                    IdFuncion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdEvento = table.Column<int>(type: "int", nullable: false),
                    IdLocal = table.Column<int>(type: "int", nullable: false),
                    FechaHora = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    EventoIdEvento = table.Column<int>(type: "int", nullable: true),
                    LocalIdLocal = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Funcion", x => x.IdFuncion);
                    table.ForeignKey(
                        name: "FK_Funcion_Evento_EventoIdEvento",
                        column: x => x.EventoIdEvento,
                        principalTable: "Evento",
                        principalColumn: "IdEvento");
                    table.ForeignKey(
                        name: "FK_Funcion_Evento_IdEvento",
                        column: x => x.IdEvento,
                        principalTable: "Evento",
                        principalColumn: "IdEvento",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Funcion_Local_LocalIdLocal",
                        column: x => x.LocalIdLocal,
                        principalTable: "Local",
                        principalColumn: "IdLocal");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Sector",
                columns: table => new
                {
                    IdSector = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Capacidad = table.Column<int>(type: "int", nullable: false),
                    IdLocal = table.Column<int>(type: "int", nullable: false),
                    LocalIdLocal = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sector", x => x.IdSector);
                    table.ForeignKey(
                        name: "FK_Sector_Local_IdLocal",
                        column: x => x.IdLocal,
                        principalTable: "Local",
                        principalColumn: "IdLocal",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sector_Local_LocalIdLocal",
                        column: x => x.LocalIdLocal,
                        principalTable: "Local",
                        principalColumn: "IdLocal",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Cliente",
                columns: table => new
                {
                    IdCliente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DNI = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nombre = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Telefono = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdUsuario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cliente", x => x.IdCliente);
                    table.ForeignKey(
                        name: "FK_Cliente_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Tarifa",
                columns: table => new
                {
                    IdTarifa = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdFuncion = table.Column<int>(type: "int", nullable: false),
                    IdSector = table.Column<int>(type: "int", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    FuncionIdFuncion = table.Column<int>(type: "int", nullable: true),
                    SectorIdSector = table.Column<int>(type: "int", nullable: true),
                    Estado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tarifa", x => x.IdTarifa);
                    table.ForeignKey(
                        name: "FK_Tarifa_Funcion_FuncionIdFuncion",
                        column: x => x.FuncionIdFuncion,
                        principalTable: "Funcion",
                        principalColumn: "IdFuncion");
                    table.ForeignKey(
                        name: "FK_Tarifa_Sector_SectorIdSector",
                        column: x => x.SectorIdSector,
                        principalTable: "Sector",
                        principalColumn: "IdSector");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Orden",
                columns: table => new
                {
                    IdOrden = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdTarifa = table.Column<int>(type: "int", nullable: false),
                    IdCliente = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    ClienteIdCliente = table.Column<int>(type: "int", nullable: false),
                    TarifaIdTarifa = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orden", x => x.IdOrden);
                    table.ForeignKey(
                        name: "FK_Orden_Cliente_ClienteIdCliente",
                        column: x => x.ClienteIdCliente,
                        principalTable: "Cliente",
                        principalColumn: "IdCliente",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orden_Cliente_IdCliente",
                        column: x => x.IdCliente,
                        principalTable: "Cliente",
                        principalColumn: "IdCliente",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orden_Tarifa_IdTarifa",
                        column: x => x.IdTarifa,
                        principalTable: "Tarifa",
                        principalColumn: "IdTarifa",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orden_Tarifa_TarifaIdTarifa",
                        column: x => x.TarifaIdTarifa,
                        principalTable: "Tarifa",
                        principalColumn: "IdTarifa",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Entrada",
                columns: table => new
                {
                    IdEntrada = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Precio = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    IdOrden = table.Column<int>(type: "int", nullable: false),
                    IdTarifa = table.Column<int>(type: "int", nullable: false),
                    IdCliente = table.Column<int>(type: "int", nullable: false),
                    IdFuncion = table.Column<int>(type: "int", nullable: false),
                    OrdenIdOrden = table.Column<int>(type: "int", nullable: false),
                    TarifaIdTarifa = table.Column<int>(type: "int", nullable: false),
                    FuncionIdFuncion = table.Column<int>(type: "int", nullable: false),
                    ClienteIdCliente = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entrada", x => x.IdEntrada);
                    table.ForeignKey(
                        name: "FK_Entrada_Cliente_ClienteIdCliente",
                        column: x => x.ClienteIdCliente,
                        principalTable: "Cliente",
                        principalColumn: "IdCliente",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Entrada_Funcion_FuncionIdFuncion",
                        column: x => x.FuncionIdFuncion,
                        principalTable: "Funcion",
                        principalColumn: "IdFuncion",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Entrada_Orden_IdOrden",
                        column: x => x.IdOrden,
                        principalTable: "Orden",
                        principalColumn: "IdOrden",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Entrada_Orden_OrdenIdOrden",
                        column: x => x.OrdenIdOrden,
                        principalTable: "Orden",
                        principalColumn: "IdOrden",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Entrada_Tarifa_IdTarifa",
                        column: x => x.IdTarifa,
                        principalTable: "Tarifa",
                        principalColumn: "IdTarifa",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Entrada_Tarifa_TarifaIdTarifa",
                        column: x => x.TarifaIdTarifa,
                        principalTable: "Tarifa",
                        principalColumn: "IdTarifa",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_IdUsuario",
                table: "Cliente",
                column: "IdUsuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Entrada_ClienteIdCliente",
                table: "Entrada",
                column: "ClienteIdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Entrada_FuncionIdFuncion",
                table: "Entrada",
                column: "FuncionIdFuncion");

            migrationBuilder.CreateIndex(
                name: "IX_Entrada_IdOrden",
                table: "Entrada",
                column: "IdOrden");

            migrationBuilder.CreateIndex(
                name: "IX_Entrada_IdTarifa",
                table: "Entrada",
                column: "IdTarifa");

            migrationBuilder.CreateIndex(
                name: "IX_Entrada_OrdenIdOrden",
                table: "Entrada",
                column: "OrdenIdOrden");

            migrationBuilder.CreateIndex(
                name: "IX_Entrada_TarifaIdTarifa",
                table: "Entrada",
                column: "TarifaIdTarifa");

            migrationBuilder.CreateIndex(
                name: "IX_Funcion_EventoIdEvento",
                table: "Funcion",
                column: "EventoIdEvento");

            migrationBuilder.CreateIndex(
                name: "IX_Funcion_IdEvento",
                table: "Funcion",
                column: "IdEvento");

            migrationBuilder.CreateIndex(
                name: "IX_Funcion_LocalIdLocal",
                table: "Funcion",
                column: "LocalIdLocal");

            migrationBuilder.CreateIndex(
                name: "IX_Orden_ClienteIdCliente",
                table: "Orden",
                column: "ClienteIdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Orden_IdCliente",
                table: "Orden",
                column: "IdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Orden_IdTarifa",
                table: "Orden",
                column: "IdTarifa");

            migrationBuilder.CreateIndex(
                name: "IX_Orden_TarifaIdTarifa",
                table: "Orden",
                column: "TarifaIdTarifa");

            migrationBuilder.CreateIndex(
                name: "IX_Sector_IdLocal",
                table: "Sector",
                column: "IdLocal");

            migrationBuilder.CreateIndex(
                name: "IX_Sector_LocalIdLocal",
                table: "Sector",
                column: "LocalIdLocal");

            migrationBuilder.CreateIndex(
                name: "IX_Tarifa_FuncionIdFuncion",
                table: "Tarifa",
                column: "FuncionIdFuncion");

            migrationBuilder.CreateIndex(
                name: "IX_Tarifa_SectorIdSector",
                table: "Tarifa",
                column: "SectorIdSector");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Entrada");

            migrationBuilder.DropTable(
                name: "Orden");

            migrationBuilder.DropTable(
                name: "Cliente");

            migrationBuilder.DropTable(
                name: "Tarifa");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "Funcion");

            migrationBuilder.DropTable(
                name: "Sector");

            migrationBuilder.DropTable(
                name: "Evento");

            migrationBuilder.DropTable(
                name: "Local");
        }
    }
}
