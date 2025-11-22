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
            lote.Estado = "Borrado";
            context.Lote.Update(lote);
            await context.SaveChangesAsync();

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
            await context.SaveChangesAsync();
            return lote.toLoteDTO();
        }

        public async Task<LoteDTO> PutLote(string Codigo, [FromBody] LoteDTO dto)
        {
            var lote = await context.Lote.FirstOrDefaultAsync(c => c.Codigo == Codigo);
            if (lote == null)
            {
                throw new Exception("Lote no encontrado");
            }
            lote.CodigoProducto = dto.CodigoProducto;
            lote.CodigoAlmacen = dto.CodigoAlmacen;
            lote.Codigo = dto.Codigo;
            lote.Cantidad = dto.Cantidad;
            lote.FechaIngreso = dto.FechaIngreso;
            lote.FechaVencimiento = dto.FechaVencimiento;
            await context.SaveChangesAsync();
            return lote.toLoteDTO();
        }
    }
}
