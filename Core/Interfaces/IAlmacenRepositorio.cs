using AlmacenLP.Core.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace AlmacenLP.Core.Interfaces
{
    public interface IAlmacenRepositorio
    {
        Task<List<AlmacenDTO>> GetAlmacen();
        Task<AlmacenDTO> GetAlmacen(string Codigo);
        Task<AlmacenDTO>PutAlmacen(string Codigo, int NuevoIdSucursal, string NuevoCodigo, string NuevoNombre, int NuevaCapacidadMaxima, int NuevaCantidadDisponible);
        Task<AlmacenDTO> PostAlmacen(int IdSucursal, string Codigo, string Nombre, int CapacidadMaxima, int CantidadDisponible);
        Task<AlmacenDTO> DeleteAlmacen(string codigo);
    }
}
