using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AlmacenLP.Migrations
{
    /// <inheritdoc />
    public partial class m2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BajaProducto");

            migrationBuilder.DropTable(
                name: "Camion");

            migrationBuilder.DropTable(
                name: "Carga");

            migrationBuilder.DropColumn(
                name: "IdAlmacen",
                table: "Lote");

            migrationBuilder.DropColumn(
                name: "IdProducto",
                table: "Lote");

            migrationBuilder.DropColumn(
                name: "IdAlmacen",
                table: "Inventario");

            migrationBuilder.DropColumn(
                name: "IdProducto",
                table: "Inventario");

            migrationBuilder.DropColumn(
                name: "IdSucursal",
                table: "Almacen");

            migrationBuilder.AddColumn<string>(
                name: "CodigoAlmacen",
                table: "Lote",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CodigoProducto",
                table: "Lote",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CodigoAlmacen",
                table: "Inventario",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CodigoLote",
                table: "Inventario",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CodigoProducto",
                table: "Inventario",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CodigoSucursal",
                table: "Almacen",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "MovimientoInventario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CodigoProducto = table.Column<string>(type: "text", nullable: false),
                    CodigoCamion = table.Column<string>(type: "text", nullable: true),
                    CodigoAlmacen = table.Column<string>(type: "text", nullable: false),
                    CodigoVenta = table.Column<string>(type: "text", nullable: true),
                    CodigoLote = table.Column<string>(type: "text", nullable: true),
                    Codigo = table.Column<string>(type: "text", nullable: false),
                    CantidadBuena = table.Column<int>(type: "integer", nullable: true),
                    CantidadMala = table.Column<int>(type: "integer", nullable: true),
                    TipoMovimiento = table.Column<string>(type: "text", nullable: false),
                    Motivo = table.Column<string>(type: "text", nullable: true),
                    Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimientoInventario", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovimientoInventario");

            migrationBuilder.DropColumn(
                name: "CodigoAlmacen",
                table: "Lote");

            migrationBuilder.DropColumn(
                name: "CodigoProducto",
                table: "Lote");

            migrationBuilder.DropColumn(
                name: "CodigoAlmacen",
                table: "Inventario");

            migrationBuilder.DropColumn(
                name: "CodigoLote",
                table: "Inventario");

            migrationBuilder.DropColumn(
                name: "CodigoProducto",
                table: "Inventario");

            migrationBuilder.DropColumn(
                name: "CodigoSucursal",
                table: "Almacen");

            migrationBuilder.AddColumn<int>(
                name: "IdAlmacen",
                table: "Lote",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdProducto",
                table: "Lote",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdAlmacen",
                table: "Inventario",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdProducto",
                table: "Inventario",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdSucursal",
                table: "Almacen",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "BajaProducto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Cantidad = table.Column<int>(type: "integer", nullable: false),
                    Codigo = table.Column<string>(type: "text", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false),
                    FechaBaja = table.Column<DateTime>(type: "date", nullable: false),
                    IdAlmacen = table.Column<int>(type: "integer", nullable: false),
                    IdProducto = table.Column<int>(type: "integer", nullable: false),
                    TipoBaja = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BajaProducto", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Camion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CantidadDisponible = table.Column<int>(type: "integer", nullable: false),
                    CapacidadMaxima = table.Column<int>(type: "integer", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false),
                    Modelo = table.Column<string>(type: "text", nullable: false),
                    Placa = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Camion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Carga",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Cantidad = table.Column<int>(type: "integer", nullable: false),
                    Codigo = table.Column<string>(type: "text", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false),
                    FechaSalida = table.Column<DateTime>(type: "date", nullable: false),
                    IdAlmacen = table.Column<int>(type: "integer", nullable: false),
                    IdCamion = table.Column<int>(type: "integer", nullable: false),
                    IdProducto = table.Column<int>(type: "integer", nullable: false),
                    IdVenta = table.Column<int>(type: "integer", nullable: false),
                    TipoMovimiento = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carga", x => x.Id);
                });
        }
    }
}
