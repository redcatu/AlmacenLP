using AlmacenLP.Core.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace AlmacenLP.Core.Interfaces
{
    public interface ILoteRepositorio
    {
        Task<List<LoteDTO>> GetLote();
        Task<LoteDTO> GetLote(string Codigo);
        Task<LoteDTO> PutLote(string Codigo, int NuevoIdProducto, int NuevoIdAlmacen, string NuevoCodigo, int Cantidad, DateTime NuevaFechaIngreso, DateTime NuevoFechaVencimiento);
        Task<LoteDTO> PostLote(int IdProducto, int IdAlmacen, string Codigo, int Cantidad, DateTime FechaIngreso, DateTime FechaVencimiento);
        Task<LoteDTO> DeleteLote(string Codigo);
    }
}
