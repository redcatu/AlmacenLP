using AlmacenLP.Core.DTOs;
using AlmacenLP.Core.Entidades;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AlmacenLP.Core.Mapeadores
{
    public static class CamionMapeador
    {
        public static CamionDTO toCamionDTO(this Camion camion)
        {
            return new CamionDTO
            {
                Placa = camion.Placa,
                Modelo = camion.Modelo,
                CapacidadMaxima = camion.CapacidadMaxima,
                CantidadDisponible = camion.CantidadDisponible
            };
        }
    }
}
