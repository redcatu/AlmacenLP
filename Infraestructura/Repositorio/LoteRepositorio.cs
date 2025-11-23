using AlmacenLP.Core.DTOs;
using AlmacenLP.Core.Entidades;
using AlmacenLP.Core.Interfaces;
using AlmacenLP.Core.Mapeadores;
using AlmacenLP.Infraestructura.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AlmacenLP.Infraestructura.Repositorio
{
    public class LoteRepositorio : ILoteRepositorio
    {
        private readonly AlmacenLPContext context;

        public LoteRepositorio(AlmacenLPContext context)
        {
            this.context = context;
        }

        public async Task<LoteDTO> DeleteLote(string Codigo)
        {
            var lote = await context.Lote.FirstOrDefaultAsync(p => p.Codigo == Codigo);
            if (lote == null)
            {
                throw new Exception("Lote no encontrado");
            }
            if (lote.Cantidad > 0)
            {
                throw new Exception($"No se puede borrar el lote {Codigo} porque aún contiene {lote.Cantidad} unidades. El stock debe ser cero.");
            }
            lote.Estado = "Borrado";
            context.Lote.Update(lote);
            await context.SaveChangesAsync();

            await RecalcularStockInventario(lote.CodigoAlmacen, lote.CodigoProducto);

            return lote.toLoteDTO();
        }

        public async Task<List<LoteDTO>> GetLote()
        {
            var lote = await (from c in context.Lote
                                 where c.Estado != "Borrado"
                                 select c
                         ).Select(ca => ca.toLoteDTO()).ToListAsync();
            return lote;
        }

        public async Task<LoteDTO> GetLote(string Codigo)
        {
            return await (from c in context.Lote
                          where c.Codigo == Codigo
                          select c.toLoteDTO()).FirstOrDefaultAsync();
        }

        public async Task<LoteDTO> PostLote([FromBody] LoteDTO dto)
        {
            if (dto.FechaVencimiento.Date <= DateTime.Today.Date)
            {
                throw new Exception("Regla L1: La fecha de vencimiento debe ser posterior al día de hoy.");
            }
            if (await context.Lote.AnyAsync(l => l.Codigo == dto.Codigo))
            {
                throw new Exception($"El código de lote '{dto.Codigo}' ya existe en el sistema.");
            }
            var almacen = await context.Almacen.FirstOrDefaultAsync(a => a.Codigo == dto.CodigoAlmacen);

            if (almacen == null)
            {
                throw new Exception("El código de almacén no existe.");
            }

            if (dto.Cantidad > almacen.CantidadDisponible)
            {
                throw new Exception($"Capacidad excedida: El almacén solo tiene {almacen.CantidadDisponible} espacio libre.");
            }

            almacen.CantidadDisponible -= dto.Cantidad;
            var lote = new Lote
            {
                CodigoProducto = dto.CodigoProducto,
                CodigoAlmacen = dto.CodigoAlmacen,
                Codigo = dto.Codigo,
                Cantidad = dto.Cantidad,
                FechaIngreso = dto.FechaIngreso,
                FechaVencimiento = dto.FechaVencimiento
            };
            context.Lote.Add(lote);
            almacen.CantidadDisponible=nuevoStockAlmacen;
            context.Almacen.Update(almacen);

            await context.SaveChangesAsync();

            await RecalcularStockInventario(lote.CodigoAlmacen, lote.CodigoProducto);

            return lote.toLoteDTO();
        }
        public async Task RecalcularStockInventario(string CodigoAlmacen, string CodigoProducto)
        {
            int StockTotal = await context.Lote.Where(l=>l.CodigoAlmacen == CodigoAlmacen && l.CodigoProducto == CodigoProducto && l.Estado =="Activo").SumAsync(l=>(int?)l.Cantidad)??0;
            var inventario = await context.Inventario.FirstOrDefaultAsync(i=>i.CodigoAlmacen==CodigoAlmacen && i.CodigoProducto==CodigoProducto);
            if(inventario != null)
            {
                inventario.ProductoStock = StockTotal;
                context.Inventario.Update(inventario);
                await context.SaveChangesAsync();
            }
        }
        public async Task<LoteDTO> PutLote(string Codigo, [FromBody] LoteDTO dto)
        {
            var lote = await context.Lote.FirstOrDefaultAsync(c => c.Codigo == Codigo);
            if (lote == null)
            {
                throw new Exception("Lote no encontrado");
            }
            if (lote.Cantidad != dto.Cantidad)
            {
                throw new Exception("No se permite modificar la Cantidad de un lote directamente. Use un Movimiento de Inventario (Entrada/Salida/Ajuste) para corregir el stock.");
            }
            if (lote.CodigoProducto != dto.CodigoProducto || lote.CodigoAlmacen != dto.CodigoAlmacen)
            {
                throw new Exception("No se permite cambiar el Producto ni el Almacén de un lote existente. Debe realizar una transferencia de inventario.");
            }
            
            lote.FechaIngreso = dto.FechaIngreso;
            lote.FechaVencimiento = dto.FechaVencimiento;
            await context.SaveChangesAsync();

            await RecalcularStockInventario(lote.CodigoAlmacen, lote.CodigoProducto);

            return lote.toLoteDTO();
        }
    }
}
