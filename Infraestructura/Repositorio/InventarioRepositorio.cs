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
    public class InventarioRepositorio : IInventarioRepositorio
    {
        private readonly AlmacenLPContext context;

        public InventarioRepositorio(AlmacenLPContext context)
        {
            this.context = context;
        }
        public async Task<InventarioDTO> DeleteInventario(string Codigo)
        {
            var inventario = await context.Inventario.FirstOrDefaultAsync(p => p.Codigo == Codigo);
            if (inventario == null)
            {
                throw new Exception("Inventario no encontrado");
            }
            inventario.Estado = "Borrado";
            context.Inventario.Update(inventario);
            await context.SaveChangesAsync();

            return inventario.toInventarioDTO();
        }

        public async Task<List<InventarioDTO>> GetInventario()
        {
            var inventario = await(from c in context.Inventario
                                where c.Estado != "Borrado"
                                select c
                         ).Select(ca => ca.toInventarioDTO()).ToListAsync();
            return inventario;
        }

        public async Task<InventarioDTO> GetInventario(string Codigo)
        {
            return await (from c in context.Inventario
                          where c.Codigo == Codigo
                          select c.toInventarioDTO()).FirstOrDefaultAsync();
        }

        public async Task<InventarioDTO> PostInventario([FromBody] InventarioDTO dto)
        {
            var inventario = new Inventario
            {
                CodigoAlmacen = dto.CodigoAlmacen,
                CodigoProducto = dto.CodigoProducto,
                Codigo = dto.Codigo,
                ProductoStock = dto.ProductoStock

            };
            context.Inventario.Add(inventario);
            await context.SaveChangesAsync();
            return inventario.toInventarioDTO();
        }

        public async Task<InventarioDTO> PutInventario(string Codigo, [FromBody] InventarioDTO dto)
        {
            var inventario = await context.Inventario.FirstOrDefaultAsync(c => c.Codigo == Codigo);
            if (inventario == null)
            {
                throw new Exception("Inventario no encontrado");
            }
            inventario.CodigoAlmacen = dto.CodigoAlmacen;
            inventario.CodigoProducto = dto.CodigoProducto;
            inventario.CodigoLote = dto.CodigoLote;
            inventario.ProductoStock = dto.ProductoStock;
            context.Inventario.Update(inventario);
            await context.SaveChangesAsync();
            return inventario.toInventarioDTO();
        }
    }
}
