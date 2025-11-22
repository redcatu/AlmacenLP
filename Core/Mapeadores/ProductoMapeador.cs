using AlmacenLP.Core.DTOs;
using AlmacenLP.Core.Entidades;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AlmacenLP.Core.Mapeadores
{
    public static class ProductoMapeador
    {
        public static ProductoDTO toProductoDTO(this Producto producto)
        {
            return new ProductoDTO()
            {
               Codigo = producto.Codigo,
               Nombre = producto.Nombre,
               Descripcion = producto.Descripcion,
               UnidadMedida = producto.UnidadMedida,
               Precio = producto.Precio,
               Marca = producto.Marca
            };
        }
        public static ProductoVentasDTO toProductoVentasDTO(this Producto producto)
        {
            return new ProductoVentasDTO()
            {
                Codigo = producto.Codigo,
                Nombre = producto.Nombre,
                Precio = producto.Precio
            };
        }
    }
}
