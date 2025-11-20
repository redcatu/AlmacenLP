using AlmacenLP.Core.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace AlmacenLP.Core.Interfaces
{
    public interface ICargaRepositorio
    {
        Task<List<CargaDTO>> GetCarga();
        Task<CargaDTO> PostCarga(int IdProducto, int IdCamion, int IdAlmacen, int IdVenta, string Codigo, int Cantidad, string TipoMovimiento, DateTime FechaSalida);
        Task<CargaDTO> PutCarga(string Codigo, int IdProductoNuevo, int IdCamionNuevo, int IdAlmacenNuevo, int IdVentaNueva, string CodigoNuevo, int CantidadNueva, string TipoMovimientoNuevo, DateTime FechaSalidaNueva);
        Task<CargaDTO> DeleteCarga(string Codigo);
    }
}
