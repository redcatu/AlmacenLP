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
                CodigoProducto = inventario.CodigoProducto,
                CodigoAlmacen = inventario.CodigoAlmacen,
                CodigoLote = inventario.CodigoLote,
                Codigo = inventario.Codigo,
                ProductoStock = inventario.ProductoStock
            };
        }
    }
}
