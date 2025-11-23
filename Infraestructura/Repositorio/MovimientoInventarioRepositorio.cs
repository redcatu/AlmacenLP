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

        public MovimientoInventarioRepositorio(AlmacenLPContext context)
        {
            this.context = context;
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

        public async Task<List<MovimientoInventarioDistribucionDTO>> GetCargaSucursal()
        {
            var movimientoInventario = await(from p in context.MovimientoInventario
                                             where p.Estado != "Borrado"
                                             select p
                          ).Select(pr => pr.toMovimientoInventarioDistribucionDTO()).ToListAsync();
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

        public async Task<MovimientoInventarioDTO> PostDevuelto([FromBody] MovimientoInventarioDTO dto)
        {
            if (string.IsNullOrEmpty(dto.CodigoLote))
            {
                throw new Exception("Una devolución debe especificar el CodigoLote original");
            }
            int cantidadBuena = dto.CantidadBuena ?? 0;
            int cantidadTotal = cantidadBuena + (dto.CantidadMala ?? 0);

            var almacen = await context.Almacen.FirstOrDefaultAsync(a => a.Codigo == dto.CodigoAlmacen);
            var inventario = await context.Inventario.FirstOrDefaultAsync(i =>
                i.CodigoAlmacen == dto.CodigoAlmacen &&
                i.CodigoProducto == dto.CodigoProducto);
            var lote = await context.Lote.FirstOrDefaultAsync(l => l.Codigo == dto.CodigoLote);

            if (almacen == null || inventario == null || lote == null)
            {
                throw new Exception("Error: Almacén, Inventario maestro o Lote original no encontrado.");
            }
            if (cantidadTotal > 0)
            {
                lote.Cantidad += cantidadBuena;
                lote.Estado = "Activo";
                context.Lote.Update(lote);
                inventario.ProductoStock += cantidadBuena;
                context.Inventario.Update(inventario);
                almacen.CantidadDisponible -= cantidadTotal;

                if (almacen.CantidadDisponible < 0)
                {
                    almacen.CantidadDisponible = 0;
                }
                context.Almacen.Update(almacen);
            }
        
                var entity = new MovimientoInventario
            {
                CodigoProducto = dto.CodigoProducto,
                CodigoCamion = dto.CodigoCamion,
                CodigoAlmacen = dto.CodigoAlmacen,
                CodigoVenta = dto.CodigoVenta,
                CodigoLote = dto.CodigoLote,
                Codigo = dto.Codigo,
                CantidadBuena = dto.CantidadBuena,
                CantidadMala = dto.CantidadMala,
                TipoMovimiento = "Devuelto",
                Motivo = dto.Motivo,
                Fecha = dto.Fecha,
            };
            context.MovimientoInventario.Add(entity);
            await context.SaveChangesAsync();
            return entity.toMovimientoInventarioDTO();
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
            int cantidadTotalMovimiento = (dto.CantidadBuena ?? 0) + (dto.CantidadMala ?? 0);

            if (cantidadTotalMovimiento > 0)
            {
                var inventario = await context.Inventario.FirstOrDefaultAsync(i =>
                    i.CodigoAlmacen == dto.CodigoAlmacen &&
                    i.CodigoProducto == dto.CodigoProducto);

                if (inventario == null)
                {
                    throw new Exception($"No existe un registro de inventario maestro para el producto {dto.CodigoProducto} en el almacén {dto.CodigoAlmacen}.");
                }
                var almacen = await context.Almacen.FirstOrDefaultAsync(a => a.Codigo == dto.CodigoAlmacen);
                if (almacen == null)
                {
                    throw new Exception("El código de almacén no existe.");
                }

                if (dto.TipoMovimiento == "Salida")
                {
                    if (cantidadTotalMovimiento > inventario.ProductoStock)
                    {
                        throw new Exception($"Error de Stock: La cantidad solicitada ({cantidadTotalMovimiento}) excede el stock disponible ({inventario.ProductoStock}) para el producto {dto.CodigoProducto}");
                    }
                    inventario.ProductoStock -= cantidadTotalMovimiento;
                    context.Inventario.Update(inventario);
                    almacen.CantidadDisponible += cantidadTotalMovimiento;
                    context.Almacen.Update(almacen);
                    await AplicarDescuentosLotes(dto.CodigoAlmacen, dto.CodigoProducto, cantidadTotalMovimiento);
                }

                else if (dto.TipoMovimiento == "Entrada" || dto.TipoMovimiento == "Devolucion")
                {                    
                    inventario.ProductoStock += cantidadTotalMovimiento;
                    context.Inventario.Update(inventario);
                    almacen.CantidadDisponible -= cantidadTotalMovimiento;
                    context.Almacen.Update(almacen);
                }
            }
            var entity = new MovimientoInventario
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

            context.MovimientoInventario.Add(entity);
            await context.SaveChangesAsync();

            return entity.toMovimientoInventarioDTO();
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
