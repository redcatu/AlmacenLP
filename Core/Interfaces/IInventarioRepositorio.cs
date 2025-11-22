using AlmacenLP.Core.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace AlmacenLP.Core.Interfaces
{
    public interface IInventarioRepositorio
    {
        Task<List<InventarioDTO>> GetInventario();
        Task<InventarioDTO> GetInventario(string Codigo);
        Task<InventarioDTO> PutInventario(string Codigo, [FromBody] InventarioDTO dto);
        Task<InventarioDTO> PostInventario([FromBody] InventarioDTO dto);
        Task<InventarioDTO> DeleteInventario(string Codigo);
    }
}
