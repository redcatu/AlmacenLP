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
        // GET: api/Ventas/Ventas
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
                // Diagnóstico mejorado:
                // Si la excepción indica un error HTTP (generado por el repositorio), 
                // podemos devolver un código de "Failed Dependency" (424) o "Service Unavailable" (503).
                if (ex.Message.StartsWith("Error HTTP"))
                {
                    // Devolvemos 503 para indicar que el servicio de ventas no está disponible o falló internamente.
                    // Esto evita que nuestro API devuelva un 500 falso.
                    return StatusCode(503, $"Error en dependencia externa (Ventas API): {ex.Message}");
                }

                // Para cualquier otro error (ej. falló la deserialización o configuración en nuestro lado)
                return StatusCode(500, $"Error interno al procesar datos: {ex.Message}");
            }
        }
    }
}
