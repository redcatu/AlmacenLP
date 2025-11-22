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
                CodigoProducto = lote.CodigoProducto,
                CodigoAlmacen = lote.CodigoAlmacen,
                Codigo = lote.Codigo,
                Cantidad = lote.Cantidad,
                FechaIngreso = lote.FechaIngreso,
                FechaVencimiento = lote.FechaVencimiento
            };
        }
    }
}
