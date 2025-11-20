using AlmacenLP.Core.DTOs;
using AlmacenLP.Core.Entidades;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AlmacenLP.Core.Mapeadores
{
    public static class InventarioMapeador
    {
        public static InventarioDTO toInventarioDTO(this Inventario inventario)
        {
            return new InventarioDTO()
            {
                IdProducto = inventario.IdProducto,
                IdAlmacen = inventario.IdAlmacen,
                Codigo = inventario.Codigo,
                ProductoStock = inventario.ProductoStock
            };
        }
    }
}
