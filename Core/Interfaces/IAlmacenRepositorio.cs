using AlmacenLP.Core.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace AlmacenLP.Core.Interfaces
{
    public interface IAlmacenRepositorio
    {
        Task<List<AlmacenDTO>> GetAlmacen();
        Task<AlmacenDTO> GetAlmacen(string Codigo);
        Task<AlmacenDTO>PutAlmacen(string Codigo, [FromBody] AlmacenDTO dto);
        Task<AlmacenDTO> PostAlmacen([FromBody] AlmacenDTO dto);
        Task<AlmacenDTO> DeleteAlmacen(string codigo);
    }
}
