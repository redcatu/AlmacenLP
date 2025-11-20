using AlmacenLP.Core.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace AlmacenLP.Core.Interfaces
{
    public interface IInventarioRepositorio
    {
        Task<List<InventarioDTO>> GetInventario();
        Task<InventarioDTO> GetInventario(string Codigo);
        Task<InventarioDTO> PutInventario(string Codigo, int NuevoIdAlmacen, int NuevoIdProducto, string NuevoCodigo, int NuevoProductoStock);
        Task<InventarioDTO> PostInventario(int IdAlmacen, int IdProducto, string Codigo, int ProductoStock);
        Task<InventarioDTO> DeleteInventario(string Codigo);
    }
}
