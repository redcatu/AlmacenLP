using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AlmacenLP.Core.Entidades;
using AlmacenLP.Infraestructura.Data;
using AlmacenLP.Core.Interfaces;

namespace AlmacenLP.Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CargasController : ControllerBase
    {
        private readonly ICargaRepositorio context;

        public CargasController(ICargaRepositorio context)
        {
            this.context = context;
        }

        // GET: api/Cargas
        [HttpGet]
        public async Task<IActionResult> GetCarga()
        {
            return Ok(await context.GetCarga());
        }

        // POST: api/Cargas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostCarga(int IdProducto, int IdCamion, int IdAlmacen, int IdVenta, string Codigo, int Cantidad, string TipoMovimiento, DateTime FechaSalida)
        {
            return Ok(await context.PostCarga(IdProducto, IdCamion, IdAlmacen, IdVenta, Codigo, Cantidad, TipoMovimiento, FechaSalida));
        }

        // PUT: api/Cargas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{Codigo}")]
        public async Task<IActionResult> PutCarga(string Codigo, int IdProductoNuevo, int IdCamionNuevo, int IdAlmacenNuevo, int IdVentaNueva, string CodigoNuevo, int CantidadNueva, string TipoMovimientoNuevo, DateTime FechaSalidaNueva)
        {
            return Ok(await context.PutCarga(Codigo, IdProductoNuevo, IdCamionNuevo, IdAlmacenNuevo, IdVentaNueva, CodigoNuevo, CantidadNueva, TipoMovimientoNuevo, FechaSalidaNueva));
        }
        // DELETE: api/Cargas/5
        [HttpDelete("{Codigo}")]
        public async Task<IActionResult> DeleteCarga(string Codigo)
        {
            return Ok(await context.DeleteCarga(Codigo));
        }
        /*
        // GET: api/Cargas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Carga>> GetCarga(int id)
        {
            var carga = await context.Carga.FindAsync(id);

            if (carga == null)
            {
                return NotFound();
            }

            return carga;
        }
        */
    }
}
