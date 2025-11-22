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

        public async Task<MovimientoInventarioDTO> PostMovimientoInventario([FromBody] MovimientoInventarioDTO dto)
        {
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
            movimientoInventario.CodigoProducto = dto.CodigoProducto;
            movimientoInventario.CodigoCamion = dto.CodigoCamion;
            movimientoInventario.CodigoAlmacen = dto.CodigoAlmacen;
            movimientoInventario.CodigoVenta = dto.CodigoVenta;
            movimientoInventario.CodigoLote = dto.CodigoLote;
            movimientoInventario.Codigo = dto.Codigo;
            movimientoInventario.CantidadBuena = dto.CantidadBuena;
            movimientoInventario.CantidadMala = dto.CantidadMala;
            movimientoInventario.TipoMovimiento = dto.TipoMovimiento;
            movimientoInventario.Motivo = dto.Motivo;
            movimientoInventario.Fecha = dto.Fecha;
            await context.SaveChangesAsync();
            return movimientoInventario.toMovimientoInventarioDTO();
        }

    }
}
