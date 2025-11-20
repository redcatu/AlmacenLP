using AlmacenLP.Core.DTOs;
using AlmacenLP.Core.Interfaces;
using AlmacenLP.Core.Mapeadores;
using AlmacenLP.Infraestructura.Data;
using Microsoft.EntityFrameworkCore;
using AlmacenLP.Core.Entidades;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AlmacenLP.Infraestructura.Repositorio
{
    public class ProductoRepositorio : IProductoRepositorio
    {
        private readonly AlmacenLPContext context;

        public ProductoRepositorio(AlmacenLPContext context)
        {
            this.context = context;
        }

        public async Task<ProductoDTO> GetProducto(string Codigo)
        {
            return await (from p in context.Producto
                          where p.Codigo == Codigo
                          select p.toProductoDTO()).FirstOrDefaultAsync();
        }
        public async Task<List<ProductoDTO>> GetProductos()
        {
            var producto = await (from p in context.Producto
                          where p.Estado != "Inactivo"
                          select p
                          ).Select(pr => pr.toProductoDTO()).ToListAsync();
            return producto;
        }
        public async Task<List<ProductoDTO>> GetProductosBorrados()
        {
            var producto = await (from p in context.Producto
                                  where p.Estado == "Inactivo"
                                  select p
                          ).Select(pr => pr.toProductoDTO()).ToListAsync();
            return producto;
        }
        public async Task<ProductoDTO> PostProducto([FromBody] ProductoDTO producto)
        {
            var entity = new Producto
            {
                Codigo = producto.Codigo,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                UnidadMedida = producto.UnidadMedida,
                Precio = producto.Precio,
                Marca = producto.Marca
            };

            context.Producto.Add(entity);
            await context.SaveChangesAsync();
            return entity.toProductoDTO();
        }

        public async Task<ProductoDTO> DeleteProducto(string codigo)
        {
            var producto = await context.Producto.FirstOrDefaultAsync(p => p.Codigo == codigo);
            if (producto == null)
            {
                throw new Exception("Producto no encontrado.");
            }
            producto.Estado = "Inactivo";
            context.Producto.Update(producto);
            await context.SaveChangesAsync();

            return producto.toProductoDTO();
        }

        public async Task<ProductoDTO> PutProducto(string codigo, ProductoDTO productoNuevo)
        {
            var productoActual = await context.Producto
                                              .FirstOrDefaultAsync(p => p.Codigo == codigo);

            if (productoActual == null)
            {
                throw new Exception("Producto no encontrado.");
            }
            productoActual.Codigo = productoNuevo.Codigo;
            productoActual.Nombre = productoNuevo.Nombre;
            productoActual.Descripcion = productoNuevo.Descripcion;
            productoActual.UnidadMedida = productoNuevo.UnidadMedida;
            productoActual.Precio = productoNuevo.Precio;
            productoActual.Marca = productoNuevo.Marca;

            context.Producto.Update(productoActual);
            await context.SaveChangesAsync();

            return productoActual.toProductoDTO();
        }
    }
}
