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

        
        private async Task AplicarDescuentosLotes(string codigoAlmacen, string codigoProducto, int cantidadTotalASacar)
        {
            var lotesActivos = await context.Lote.Where(l => l.CodigoAlmacen == codigoAlmacen &&
            l.CodigoProducto == codigoProducto &&
            l.Estado == "Activo" && l.Cantidad > 0).OrderBy(l => l.FechaVencimiento).ToListAsync();

            int cantidadRestante = cantidadTotalASacar;

            foreach (var lote in lotesActivos)
            {
                if (cantidadRestante <= 0) break;
                int cantidadDescontarDeEsteLote = Math.Min(lote.Cantidad, cantidadRestante);

                lote.Cantidad -= cantidadDescontarDeEsteLote;
                cantidadRestante -= cantidadDescontarDeEsteLote;

                if (lote.Cantidad == 0)
                {
                    lote.Estado = "Terminado";
                }
                context.Lote.Update(lote);
            }
            
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
                // Lógica Crítica de Salida / Venta (FIFO)
                await ProcesarSalidaFIFO(movimiento, inventario, almacen);
            }
            else if (movimiento.TipoMovimiento == "Entrada")
            {
                // Lógica de Entrada Genérica (Creación de Lote para Compras/Ingresos)
                await ProcesarEntradaGenerica(movimiento, almacen);
            }
            else if (movimiento.TipoMovimiento == "Devuelto")
            {
                // Lógica de Devolución (añade stock bueno al lote y consume capacidad por stock total)
                await ProcesarDevolucion(movimiento, almacen);
            }
            context.MovimientoInventario.Add(movimiento);
            context.Almacen.Update(almacen);

            await context.SaveChangesAsync();

            // 6. Sincronizar stock del Inventario (Regla B2)
            // Esto asegura que el ProductoStock en Inventario sea la suma actual de todos los lotes restantes.
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

            // Regla B4: Validar capacidad (Espacio Libre) para el total devuelto.
            if (cantidadTotal > almacen.CantidadDisponible)
            {
                throw new Exception($"Capacidad excedida: El almacén solo tiene {almacen.CantidadDisponible} espacio libre para el total de la devolución ({cantidadTotal}).");
            }

            if (cantidadTotal > 0)
            {
                // 1. Devolver solo la Cantidad Buena al Lote
                lote.Cantidad += cantidadBuena;
                lote.Estado = "Activo"; // Aseguramos que el lote esté activo si regresa stock.
                context.Lote.Update(lote);

                // 2. Ocupar el espacio: El espacio libre DISMINUYE por la Cantidad Total (Buena + Mala)
                // Esto considera que el stock malo ocupa espacio de cuarentena/descarte.
                almacen.CantidadDisponible -= cantidadTotal;

                // Nota: El Inventario.ProductoStock será recalculado por la Regla B2 al final del PostMovimientoInventario.
            }
        }
        private async Task ProcesarEntradaGenerica(MovimientoInventario movimiento, Almacen almacen)
        {
            // La entrada genérica crea un nuevo lote.
            int cantidadIngresada = movimiento.CantidadBuena ?? 0;

            // Regla B4: Validar capacidad (Espacio Libre)
            if (cantidadIngresada > almacen.CantidadDisponible)
            {
                throw new Exception($"Capacidad excedida: El almacén solo tiene {almacen.CantidadDisponible} espacio libre para el ingreso.");
            }

            // Ocupar el espacio: El espacio libre DISMINUYE 
            almacen.CantidadDisponible -= cantidadIngresada;

            // Crear un código de lote único.
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
                FechaVencimiento = DateTime.Today.AddYears(1), // Fecha de vencimiento genérica 
                Estado = "Activo"
            };

            context.Lote.Add(lote);
        }

        // -----------------------------------------------------------------
        // Lógica de Salida FIFO (Regla B7)
        // -----------------------------------------------------------------
        private async Task ProcesarSalidaFIFO(MovimientoInventario movimiento, Inventario inventario, Almacen almacen)
        {
            int cantidadRequerida = movimiento.CantidadBuena ?? 0;

            // Regla S1: Validación de stock
            if (inventario.ProductoStock < cantidadRequerida)
            {
                throw new Exception($"Stock insuficiente: Solo quedan {inventario.ProductoStock} unidades en el inventario.");
            }

            // Regla B7: Seleccionar Lotes (FIFO: Ordenar por fecha de ingreso ascendente)
            var lotes = await context.Lote
                .Where(l => l.CodigoAlmacen == movimiento.CodigoAlmacen && l.CodigoProducto == movimiento.CodigoProducto && l.Cantidad > 0)
                .OrderBy(l => l.FechaIngreso) // FIFO
                .ToListAsync();

            if (lotes.Count == 0)
            {
                throw new Exception("Error interno: Inventario dice tener stock, pero no se encontraron lotes activos.");
            }

            // Recorrer lotes y descontar la cantidad
            foreach (var lote in lotes)
            {
                if (cantidadRequerida <= 0) break; // Ya se cubrió toda la venta

                int cantidadADescontar = Math.Min(lote.Cantidad, cantidadRequerida);

                // 1. Descontar del Lote
                lote.Cantidad -= cantidadADescontar;

                // 2. Liberar espacio en el Almacén (Regla A8: el espacio libre AUMENTA)
                almacen.CantidadDisponible += cantidadADescontar;

                // 3. Reducir la cantidad pendiente a vender
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
