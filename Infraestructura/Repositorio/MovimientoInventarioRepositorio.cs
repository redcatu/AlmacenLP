using AlmacenLP.Core.DTOs;
using AlmacenLP.Core.Entidades;
using AlmacenLP.Core.Interfaces;
using AlmacenLP.Core.Mapeadores;
using AlmacenLP.Infraestructura.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlmacenLP.Infraestructura.Repositorio
{
    public class MovimientoInventarioRepositorio : IMovimientoInventarioRepositorio
    {
        private readonly AlmacenLPContext context;
        private readonly ILoteRepositorio loteRepositorio;

        public MovimientoInventarioRepositorio(AlmacenLPContext context)
        {
            this.context = context;
            this.loteRepositorio = new LoteRepositorio(context);
        }

        public async Task<MovimientoInventarioDTO> DeleteMovimientoInventario(string Codigo)
        {
            var movimientoInventario = await context.MovimientoInventario.FirstOrDefaultAsync(p => p.Codigo == Codigo);
            if (movimientoInventario == null)
            {
                throw new Exception("Movimiento de inventario no encontrado");
            }
            
            movimientoInventario.Estado = "Borrado";
            context.MovimientoInventario.Update(movimientoInventario);
            await context.SaveChangesAsync();

            return movimientoInventario.toMovimientoInventarioDTO();
        }

        public async Task<List<MovimientoInventarioDistribucionDTO>> GetCargaDistribucion()
        {
            var movimientoInventario = await(from p in context.MovimientoInventario
                                             where p.Estado != "Borrado" && p.TipoMovimiento == "Salida"
                                             select p
                          ).Select(pr => pr.toMovimientoInventarioDistribucionDTO()).ToListAsync();
            return movimientoInventario;
        }

        public async Task<List<MovimientoInventarioSucursalDTO>> GetCargaSucursal()
        {
            var movimientoInventario = await(from p in context.MovimientoInventario
                                             where p.Estado != "Borrado"
                                             select p
                          ).Select(pr => pr.toMovimientoInventarioSucursalDTO()).ToListAsync();
            return movimientoInventario;
        }

        public async Task<List<MovimientoInventarioDTO>> GetMovimientoInventario()
        {
            var movimientoInventario = await (from p in context.MovimientoInventario
                                  where p.Estado != "Borrado"
                                  select p
                          ).Select(pr => pr.toMovimientoInventarioDTO()).ToListAsync();
            return movimientoInventario;
        }

        public async Task<MovimientoInventarioDTO> GetMovimientoInventario(string Codigo)
        {
            return await (from p in context.MovimientoInventario
                          where p.Codigo == Codigo
                          select p.toMovimientoInventarioDTO()).FirstOrDefaultAsync();
        }
        public async Task<MovimientoInventarioDTO> PostMovimientoInventario([FromBody] MovimientoInventarioDTO dto)
        {
            var almacen = await context.Almacen.FirstOrDefaultAsync(a => a.Codigo == dto.CodigoAlmacen);
            var inventario = await context.Inventario
                .FirstOrDefaultAsync(i => i.CodigoAlmacen == dto.CodigoAlmacen && i.CodigoProducto == dto.CodigoProducto);

            if (almacen == null) throw new Exception("El código de almacén no existe.");
            if (inventario == null) throw new Exception("No existe registro de inventario para este producto/almacén. Debe inicializarlo primero.");
            if (await context.MovimientoInventario.AnyAsync(m => m.Codigo == dto.Codigo))
            {
                throw new Exception($"El código de movimiento '{dto.Codigo}' ya existe en el sistema.");
            }

            var movimiento = new MovimientoInventario
            {
                CodigoProducto = dto.CodigoProducto,
                CodigoCamion = dto.CodigoCamion,
                CodigoAlmacen = dto.CodigoAlmacen,
                CodigoVenta = dto.CodigoVenta,
                CodigoLote = dto.CodigoLote,
                Codigo = dto.Codigo,
                CantidadBuena = dto.CantidadBuena,
                CantidadMala = dto.CantidadMala,
                TipoMovimiento = dto.TipoMovimiento,
                Motivo = dto.Motivo,
                Fecha = dto.Fecha,
            };
            if (movimiento.TipoMovimiento == "Salida")
            {
                
                await ProcesarSalidaFIFO(movimiento, inventario, almacen);
            }
            else if (movimiento.TipoMovimiento == "Entrada")
            {
                await ProcesarEntradaGenerica(movimiento, almacen);
            }
            else if (movimiento.TipoMovimiento == "Devuelto")
            {
                await ProcesarDevolucion(movimiento, almacen);
            }
            context.MovimientoInventario.Add(movimiento);
            context.Almacen.Update(almacen);

            await context.SaveChangesAsync();

            await loteRepositorio.RecalcularStockInventario(movimiento.CodigoAlmacen, movimiento.CodigoProducto);

            return movimiento.toMovimientoInventarioDTO();
        }
        private async Task ProcesarDevolucion(MovimientoInventario movimiento, Almacen almacen)
        {
            if (string.IsNullOrEmpty(movimiento.CodigoLote))
            {
                throw new Exception("Una devolución (Devuelto) debe especificar el CodigoLote original al que se devuelve el stock.");
            }

            int cantidadBuena = movimiento.CantidadBuena ?? 0;
            int cantidadMala = movimiento.CantidadMala ?? 0;
            int cantidadTotal = cantidadBuena + cantidadMala;

            var lote = await context.Lote.FirstOrDefaultAsync(l => l.Codigo == movimiento.CodigoLote);

            if (lote == null)
            {
                throw new Exception($"Error: Lote original '{movimiento.CodigoLote}' no encontrado.");
            }

            if (cantidadTotal > almacen.CantidadDisponible)
            {
                throw new Exception($"Capacidad excedida: El almacén solo tiene {almacen.CantidadDisponible} espacio libre para el total de la devolución ({cantidadTotal}).");
            }

            if (cantidadTotal > 0)
            {
                lote.Cantidad += cantidadBuena;
                lote.Estado = "Activo";
                context.Lote.Update(lote);

                almacen.CantidadDisponible -= cantidadTotal;

            }
        }
        private async Task ProcesarEntradaGenerica(MovimientoInventario movimiento, Almacen almacen)
        {
            
            int cantidadIngresada = movimiento.CantidadBuena ?? 0;

            
            if (cantidadIngresada > almacen.CantidadDisponible)
            {
                throw new Exception($"Capacidad excedida: El almacén solo tiene {almacen.CantidadDisponible} espacio libre para el ingreso.");
            }

            
            almacen.CantidadDisponible -= cantidadIngresada;

            
            string nuevoCodigoLote = $"LOTE-ING-{movimiento.Codigo}";

            if (await context.Lote.AnyAsync(l => l.Codigo == nuevoCodigoLote))
            {
                throw new Exception($"Error: El lote de ingreso {nuevoCodigoLote} ya existe.");
            }

            var lote = new Lote
            {
                CodigoProducto = movimiento.CodigoProducto,
                CodigoAlmacen = movimiento.CodigoAlmacen,
                Codigo = nuevoCodigoLote,
                Cantidad = cantidadIngresada,
                FechaIngreso = movimiento.Fecha,
                FechaVencimiento = DateTime.Today.AddYears(1),
                Estado = "Activo"
            };

            context.Lote.Add(lote);
        }

        private async Task ProcesarSalidaFIFO(MovimientoInventario movimiento, Inventario inventario, Almacen almacen)
        {
            int cantidadRequerida = movimiento.CantidadBuena ?? 0;

            if (inventario.ProductoStock < cantidadRequerida)
            {
                throw new Exception($"Stock insuficiente: Solo quedan {inventario.ProductoStock} unidades en el inventario.");
            }

            var lotes = await context.Lote
                .Where(l => l.CodigoAlmacen == movimiento.CodigoAlmacen && l.CodigoProducto == movimiento.CodigoProducto && l.Cantidad > 0)
                .OrderBy(l => l.FechaIngreso)
                .ToListAsync();

            if (lotes.Count == 0)
            {
                throw new Exception("Error interno: Inventario dice tener stock, pero no se encontraron lotes activos.");
            }

            foreach (var lote in lotes)
            {
                if (cantidadRequerida <= 0) break;

                int cantidadADescontar = Math.Min(lote.Cantidad, cantidadRequerida);

                lote.Cantidad -= cantidadADescontar;

                almacen.CantidadDisponible += cantidadADescontar;

                cantidadRequerida -= cantidadADescontar;

                context.Lote.Update(lote);
            }
        }
        public async Task<MovimientoInventarioDTO> PutMovimientoInventario(string Codigo, [FromBody] MovimientoInventarioDTO dto)
        {
            var movimientoInventario = await context.MovimientoInventario.FirstOrDefaultAsync(c => c.Codigo == Codigo);

            if (movimientoInventario == null)
            {
                throw new Exception("Movimiento de inventario no encontrado");
            }
            if (movimientoInventario.Estado == "Borrado")
            {
                throw new Exception($"El movimiento de inventario {Codigo} está Borrado y no se puede actualizar.");
            }

            if (movimientoInventario.CantidadBuena != dto.CantidadBuena ||
                movimientoInventario.CantidadMala != dto.CantidadMala ||
                movimientoInventario.TipoMovimiento != dto.TipoMovimiento ||
                movimientoInventario.CodigoProducto != dto.CodigoProducto ||
                movimientoInventario.CodigoAlmacen != dto.CodigoAlmacen)
            {
                throw new Exception("No se permite modificar cantidades, tipo de movimiento o códigos de producto/almacén en un movimiento ya registrado. Debe crear un movimiento de corrección.");
            }
            if (movimientoInventario.Motivo != dto.Motivo)
            {
                movimientoInventario.Motivo = dto.Motivo;
            }
            await context.SaveChangesAsync();

            return movimientoInventario.toMovimientoInventarioDTO();
        }

    }
}
