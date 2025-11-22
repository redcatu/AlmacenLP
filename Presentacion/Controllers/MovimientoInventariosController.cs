using AlmacenLP.Core.DTOs;
using AlmacenLP.Core.Entidades;
using AlmacenLP.Core.Interfaces;
using AlmacenLP.Infraestructura.Data;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlmacenLP.Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovimientoInventariosController : ControllerBase
    {
        private readonly IMovimientoInventarioRepositorio context;

        public MovimientoInventariosController(IMovimientoInventarioRepositorio context)
        {
            this.context = context;
        }

        // GET: api/MovimientoInventarios
        [HttpGet]
        public async Task<IActionResult> GetMovimientoInventario()
        {
            return Ok(await context.GetMovimientoInventario());
        }
        // GET: api/MovimientoInventarios
        [HttpGet("Distribucion")]
        public async Task<IActionResult> GetCargaDistribucion()
        {
            return Ok(await context.GetCargaDistribucion());
        }
        // GET: api/MovimientoInventarios
        [HttpGet("Sucursal")]
        public async Task<IActionResult> GetCargaSucursal()
        {
            return Ok(await context.GetCargaSucursal());
        }
        // GET: api/MovimientoInventarios/5
        [HttpGet("{Codigo}")]
        public async Task<IActionResult> GetMovimientoInventario(string Codigo)
        {

            return Ok(await context.GetMovimientoInventario(Codigo));
        }

        // PUT: api/MovimientoInventarios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{Codigo}")]
        public async Task<IActionResult> PutMovimientoInventario(string Codigo, [FromBody] MovimientoInventarioDTO dto)
        {
            MovimientoInventarioDTO producto = await context.PutMovimientoInventario(Codigo, dto);
            if (producto == null)
            {
                return NotFound();
            }
            return Ok(producto);
        }

        // POST: api/MovimientoInventarios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostMovimientoInventario([FromBody] MovimientoInventarioDTO dto)
        {
            return Ok(await context.PostMovimientoInventario(dto));
        }

        // DELETE: api/MovimientoInventarios/5
        [HttpDelete("{Codigo}")]
        public async Task<IActionResult> DeleteMovimientoInventario(string Codigo)
        {
            return Ok(await context.DeleteMovimientoInventario(Codigo));
        }
    }
}
