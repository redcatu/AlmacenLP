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
    public class LotesController : ControllerBase
    {
        private readonly ILoteRepositorio context;

        public LotesController(ILoteRepositorio context)
        {
            this.context = context;
        }

        // GET: api/Lotes
        [HttpGet]
        public async Task<IActionResult> GetLote()
        {
            return Ok(await context.GetLote());
        }

        // GET: api/Lotes/5
        [HttpGet("{Codigo}")]
        public async Task<IActionResult> GetLote(string Codigo)
        {
            return Ok(await context.GetLote(Codigo));
        }

        // PUT: api/Lotes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{Codigo}")]
        public async Task<IActionResult> PutLote(string Codigo, int NuevoIdProducto, int NuevoIdAlmacen, string NuevoCodigo, int NuevaCantidad, DateTime NuevaFechaIngreso, DateTime NuevoFechaVencimiento)
        {
            return Ok(await context.PutLote(Codigo, NuevoIdProducto, NuevoIdAlmacen, NuevoCodigo, NuevaCantidad, NuevaFechaIngreso, NuevoFechaVencimiento));
        }

        // POST: api/Lotes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostLote(int IdProducto, int IdAlmacen, string Codigo, int Cantidad, DateTime FechaIngreso, DateTime FechaVencimiento)
        {
            return Ok(await context.PostLote(IdProducto, IdAlmacen, Codigo, Cantidad, FechaIngreso, FechaVencimiento));
        }

        // DELETE: api/Lotes/5
        [HttpDelete("{Codigo}")]
        public async Task<IActionResult> DeleteLote(string Codigo)
        {
            return Ok(await context.DeleteLote(Codigo));
        }
    }
}
