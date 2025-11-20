using AlmacenLP.Core.DTOs;
using AlmacenLP.Core.Entidades;

namespace AlmacenLP.Core.Mapeadores
{
    public static class BajaProductoMapeador
    {
        public static BajaProductoDTO toBajaProductoDTO(this BajaProducto bajaProducto)
        {
            return new BajaProductoDTO
            {
                IdProducto = bajaProducto.IdProducto,
                IdAlmacen = bajaProducto.IdAlmacen,
                Codigo = bajaProducto.Codigo,
                Cantidad = bajaProducto.Cantidad,
                TipoBaja = bajaProducto.TipoBaja,
                FechaBaja = bajaProducto.FechaBaja
            };
        }
    }
}
