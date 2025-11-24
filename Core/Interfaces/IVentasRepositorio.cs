using AlmacenLP.Core.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace AlmacenLP.Core.Interfaces
{
    public interface IVentasRepositorio
    {
        Task<List<VentaDTO>> GetProductoCantidad();
    }
}
