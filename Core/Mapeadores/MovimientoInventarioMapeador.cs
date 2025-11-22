using AlmacenLP.Core.DTOs;
using AlmacenLP.Core.Entidades;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AlmacenLP.Core.Mapeadores
{
    public static class MovimientoInventarioMapeador
    {
        public static MovimientoInventarioDTO toMovimientoInventarioDTO(this MovimientoInventario movimientoInventario)
        {
            return new MovimientoInventarioDTO() 
            {
                CodigoProducto = movimientoInventario.CodigoProducto,
                CodigoCamion = movimientoInventario.CodigoCamion,
                CodigoAlmacen = movimientoInventario.CodigoAlmacen,
                CodigoVenta = movimientoInventario.CodigoVenta,
                CodigoLote = movimientoInventario.CodigoLote,
                Codigo = movimientoInventario.Codigo,
                CantidadBuena = movimientoInventario.CantidadBuena,
                CantidadMala = movimientoInventario.CantidadMala,
                TipoMovimiento = movimientoInventario.TipoMovimiento,
                Motivo = movimientoInventario.Motivo,
                Fecha = movimientoInventario.Fecha
            };
        }
        public static MovimientoInventarioDistribucionDTO toMovimientoInventarioDistribucionDTO(this MovimientoInventario movimientoInventario)
        {
            return new MovimientoInventarioDistribucionDTO()
            {
                CodigoProducto = movimientoInventario.CodigoProducto,
                Codigo = movimientoInventario.Codigo,
                CantidadBuena = movimientoInventario.CantidadBuena
            };
        }
        public static MovimientoInventarioSucursalDTO toMovimientoInventarioSucursalDTO(this MovimientoInventario movimientoInventario)
        {
            return new MovimientoInventarioSucursalDTO()
            {
                CodigoProducto = movimientoInventario.CodigoProducto,
                Codigo = movimientoInventario.Codigo,
                CantidadBuena = movimientoInventario.CantidadBuena,
                CantidadMala = movimientoInventario.CantidadMala,
                TipoMovimiento = movimientoInventario.TipoMovimiento,
                Motivo = movimientoInventario.Motivo,
                Fecha = movimientoInventario.Fecha
            };
        }
    }
}