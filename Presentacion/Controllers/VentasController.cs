using AlmacenLP.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AlmacenLP.Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentasController : ControllerBase
    {
        private readonly IVentasRepositorio ventasRepositorio;
        public VentasController(IVentasRepositorio ventasRepositorio)
        {
            this.ventasRepositorio = ventasRepositorio;
        }
        [HttpGet("Ventas")]
        public async Task<IActionResult> GetProductoCantidad()
        {
            try
            {
                var datos = await ventasRepositorio.GetProductoCantidad();
                return Ok(datos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener datos: {ex.Message}");
            }
        }
    }
}
