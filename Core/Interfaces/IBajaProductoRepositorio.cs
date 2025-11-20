using AlmacenLP.Core.DTOs;
using AlmacenLP.Core.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace AlmacenLP.Core.Interfaces
{
    public interface IBajaProductoRepositorio
    {
        Task<List<BajaProductoDTO>> GetBajaProducto();
        Task<BajaProductoDTO> GetBajaProducto(string Codigo);
        Task<BajaProductoDTO> PutBajaProducto(string Codigo, int NuevoIdProducto, int NuevoIdAlmacen, string NuevoCodigo, int NuevaCantidad, string NuevoTipoBaja, DateTime NuevaFechaBaja);
        Task<BajaProductoDTO> PostBajaProducto(int IdProducto, int IdAlmacen, string Codigo, int Cantidad, string TipoBaja, DateTime FechaBaja);
        Task<BajaProductoDTO> DeleteBajaProducto(string Codigo);
    }
}
