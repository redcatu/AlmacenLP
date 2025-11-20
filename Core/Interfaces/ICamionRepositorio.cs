using AlmacenLP.Core.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace AlmacenLP.Core.Interfaces
{
    public interface ICamionRepositorio
    {
        Task<List<CamionDTO>> GetCamiones();
        Task<CamionDTO> PostCamion(string Placa, string Modelo, int CapacidadMaxima, int CantidadDisponible);
        Task<CamionDTO> GetCamion(string Placa);
        Task<CamionDTO> PutCamion(string Placa, string NuevaPlaca, string NuevoModelo, int NuevaCapacidadMaxima, int NuevaCantidadDisponible);
        Task<CamionDTO> DeleteCamion(string Placa);
        bool CamionExists(string Placa);
    }
}
