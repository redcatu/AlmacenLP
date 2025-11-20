using AlmacenLP.Core.DTOs;
using AlmacenLP.Core.Entidades;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AlmacenLP.Core.Mapeadores
{
    public static class CargaMapeador
    {
        public static CargaDTO toCargaDTO(this Carga carga)
        {
            return new CargaDTO()
            {
                IdProducto = carga.IdProducto,
                IdCamion = carga.IdCamion,
                IdAlmacen = carga.IdAlmacen,
                IdVenta = carga.IdVenta,
                Codigo = carga.Codigo,
                Cantidad = carga.Cantidad,
                TipoMovimiento = carga.TipoMovimiento,
                FechaSalida = carga.FechaSalida
            };
        }
    }
}
