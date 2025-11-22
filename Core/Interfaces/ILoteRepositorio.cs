using AlmacenLP.Core.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace AlmacenLP.Core.Interfaces
{
    public interface ILoteRepositorio
    {
        Task<List<LoteDTO>> GetLote();
        Task<LoteDTO> GetLote(string Codigo);
        Task<LoteDTO> PutLote(string Codigo, [FromBody] LoteDTO dto);
        Task<LoteDTO> PostLote([FromBody] LoteDTO dto);
        Task<LoteDTO> DeleteLote(string Codigo);
    }
}
