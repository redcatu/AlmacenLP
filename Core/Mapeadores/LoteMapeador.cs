using AlmacenLP.Core.DTOs;
using AlmacenLP.Core.Entidades;

namespace AlmacenLP.Core.Mapeadores
{
    public static class LoteMapeador
    {
        public static LoteDTO toLoteDTO(this Lote lote)
        {
            return new LoteDTO()
            {
                IdProducto = lote.IdProducto,
                IdAlmacen = lote.IdAlmacen,
                Codigo = lote.Codigo,
                Cantidad = lote.Cantidad,
                FechaIngreso = lote.FechaIngreso,
                FechaVencimiento = lote.FechaVencimiento
            };
        }
    }
}
