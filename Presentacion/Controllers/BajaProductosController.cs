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
    public class BajaProductosController : ControllerBase
    {
        private readonly IBajaProductoRepositorio context;

        public BajaProductosController(IBajaProductoRepositorio context)
        {
            this.context = context;
        }

        // GET: api/BajaProductos
        [HttpGet]
        public async Task<IActionResult> GetBajaProducto()
        {
            return Ok(await context.GetBajaProducto());
        }

        // GET: api/BajaProductos/5
        [HttpGet("{Codigo}")]
        public async Task<IActionResult> GetBajaProducto(string Codigo)
        {
            return Ok(await context.GetBajaProducto(Codigo));
        }

        // PUT: api/BajaProductos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{Codigo}")]
        public async Task<IActionResult> PutBajaProducto(string Codigo, int NuevoIdProducto, int NuevoIdAlmacen, string NuevoCodigo, int NuevaCantidad, string NuevoTipoBaja, DateTime NuevaFechaBaja)
        {
            return Ok(await context.PutBajaProducto(Codigo, NuevoIdProducto, NuevoIdAlmacen, NuevoCodigo, NuevaCantidad, NuevoTipoBaja, NuevaFechaBaja));
        }

        // POST: api/BajaProductos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostBajaProducto(int IdProducto, int IdAlmacen, string Codigo, int Cantidad, string TipoBaja, DateTime FechaBaja)
        {
            return Ok(await context.PostBajaProducto(IdProducto, IdAlmacen, Codigo, Cantidad, TipoBaja, FechaBaja));
        }

        // DELETE: api/BajaProductos/5
        [HttpDelete("{Codigo}")]
        public async Task<IActionResult> DeleteBajaProducto(string Codigo)
        {
            return Ok(await context.DeleteBajaProducto(Codigo));
        }
    }
}
