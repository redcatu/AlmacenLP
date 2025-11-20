using AlmacenLP.Core.DTOs;
using AlmacenLP.Core.Entidades;
using AlmacenLP.Core.Interfaces;
using AlmacenLP.Core.Mapeadores;
using AlmacenLP.Infraestructura.Data;
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
            lote.Estado = "No disponible";
            context.Lote.Update(lote);
            await context.SaveChangesAsync();

            return lote.toLoteDTO();
        }

        public async Task<List<LoteDTO>> GetLote()
        {
            var lote = await (from c in context.Lote
                                 where c.Estado != "No disponible"
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

        public async Task<LoteDTO> PostLote(int IdProducto, int IdAlmacen, string Codigo, int Cantidad, DateTime FechaIngreso, DateTime FechaVencimiento)
        {
            var lote = new Lote
            {
                IdProducto = IdProducto,
                IdAlmacen = IdAlmacen,
                Codigo = Codigo,
                Cantidad = Cantidad,
                FechaIngreso = FechaIngreso,
                FechaVencimiento = FechaVencimiento
            };
            context.Lote.Add(lote);
            await context.SaveChangesAsync();
            return lote.toLoteDTO();
        }

        public async Task<LoteDTO> PutLote(string Codigo, int NuevoIdProducto, int NuevoIdAlmacen, string NuevoCodigo, int Cantidad, DateTime NuevaFechaIngreso, DateTime NuevoFechaVencimiento)
        {
            var lote = await context.Lote.FirstOrDefaultAsync(c => c.Codigo == Codigo);
            if (lote == null)
            {
                throw new Exception("Lote no encontrado");
            }
            lote.IdProducto = NuevoIdProducto;
            lote.IdAlmacen = NuevoIdAlmacen;
            lote.Codigo = NuevoCodigo;
            lote.Cantidad = Cantidad;
            lote.FechaIngreso = NuevaFechaIngreso;
            lote.FechaVencimiento = NuevoFechaVencimiento;
            await context.SaveChangesAsync();
            return lote.toLoteDTO();
        }
    }
}
