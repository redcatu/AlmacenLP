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
            if (inventario.ProductoStock > 0)
            {
                throw new Exception($"No se puede dar de baja el producto del almacén porque aún tiene {inventario.ProductoStock} unidades de stock.");
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
            var existeProducto = await context.Producto.AnyAsync(p => p.Codigo == dto.CodigoProducto);
            if (!existeProducto)
                throw new Exception($"El producto {dto.CodigoProducto} no existe.");

            var existeAlmacen = await context.Almacen.AnyAsync(a => a.Codigo == dto.CodigoAlmacen);
            if (!existeAlmacen)
                throw new Exception($"El almacén {dto.CodigoAlmacen} no existe.");

            var existeInventario = await context.Inventario.AnyAsync(i=> i.CodigoAlmacen == dto.CodigoAlmacen && i.CodigoProducto == dto.CodigoProducto);
            if (existeInventario)
            {
                throw new Exception("Ya existe un registro de Inventario para este producto en este almacén. Use la función de actualización.");
            }

            var inventario = new Inventario
            {
                CodigoAlmacen = dto.CodigoAlmacen,
                CodigoProducto = dto.CodigoProducto,
                CodigoLote = dto.CodigoLote,
                Codigo = dto.Codigo,
                ProductoStock = 0

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
            if (inventario.ProductoStock != dto.ProductoStock)
            {   
                throw new Exception("No se puede modificar el Stock manualmente. Use Movimientos de Inventario o Ajustes.");
            }
            if (inventario.CodigoProducto != dto.CodigoProducto || inventario.CodigoAlmacen != dto.CodigoAlmacen)
            {
                throw new Exception("No se permite cambiar el Producto ni el Almacén de un registro de inventario existente.");
            }
            inventario.CodigoLote = dto.CodigoLote;
            
            context.Inventario.Update(inventario);
            await context.SaveChangesAsync();
            return inventario.toInventarioDTO();
        }
    }
}
