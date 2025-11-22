using AlmacenLP.Core.DTOs;
using AlmacenLP.Core.Entidades;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AlmacenLP.Core.Mapeadores
{
    public static class AlmacenMapeador
    {
        public static AlmacenDTO toAlmacenDTO(this Almacen almacen)
        {
            return new AlmacenDTO()
            {
                CodigoSucursal = almacen.CodigoSucursal,
                Codigo = almacen.Codigo,
                Nombre = almacen.Nombre,
                CapacidadMaxima = almacen.CapacidadMaxima,
                CantidadDisponible = almacen.CantidadDisponible
            };
                
        }
    }
}
