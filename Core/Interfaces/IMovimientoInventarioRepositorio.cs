using AlmacenLP.Core.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace AlmacenLP.Core.Interfaces
{
    public interface IMovimientoInventarioRepositorio
    {
        Task<List<MovimientoInventarioDTO>> GetMovimientoInventario();
        Task<MovimientoInventarioDTO> GetMovimientoInventario(string Codigo);
        Task<MovimientoInventarioDTO> PutMovimientoInventario(string Codigo, [FromBody] MovimientoInventarioDTO dto);
        Task<MovimientoInventarioDTO> PostMovimientoInventario([FromBody] MovimientoInventarioDTO dto);
        Task<MovimientoInventarioDTO> DeleteMovimientoInventario(string Codigo);
        Task<List<MovimientoInventarioDistribucionDTO>> GetCargaDistribucion();
        Task<List<MovimientoInventarioDistribucionDTO>> GetCargaSucursal();
    }
}
