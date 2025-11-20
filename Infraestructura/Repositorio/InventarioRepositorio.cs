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
                throw new Exception("Almacen no encontrado");
            }
            inventario.Estado = "Inactivo";
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

        public async Task<InventarioDTO> PostInventario(int IdAlmacen, int IdProducto, string Codigo, int ProductoStock)
        {
            var inventario = new Inventario
            {
                IdAlmacen = IdAlmacen,
                IdProducto = IdProducto,
                Codigo = Codigo,
                ProductoStock = ProductoStock

            };
            context.Inventario.Add(inventario);
            await context.SaveChangesAsync();
            return inventario.toInventarioDTO();
        }

        public async Task<InventarioDTO> PutInventario(string Codigo, int NuevoIdAlmacen, int NuevoIdProducto, string NuevoCodigo, int NuevoProductoStock)
        {
            var inventario = await context.Inventario.FirstOrDefaultAsync(c => c.Codigo == Codigo);
            if (inventario == null)
            {
                throw new Exception("Inventario no encontrado");
            }
            context.Inventario.Update(inventario);
            await context.SaveChangesAsync();
            return inventario.toInventarioDTO();
        }
    }
}
