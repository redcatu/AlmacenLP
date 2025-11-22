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
using AlmacenLP.Core.DTOs;

namespace AlmacenLP.Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventariosController : ControllerBase
    {
        private readonly IInventarioRepositorio context;

        public InventariosController(IInventarioRepositorio context)
        {
            this.context = context;
        }

        // GET: api/Inventarios
        [HttpGet]
        public async Task<IActionResult> GetInventario()
        {
            return Ok(await context.GetInventario());
        }

        // GET: api/Inventarios/5
        [HttpGet("{Codigo}")]
        public async Task<IActionResult> GetInventario(string Codigo)
        {
            return Ok(await context.GetInventario(Codigo));
        }

        // PUT: api/Inventarios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{Codigo}")]
        public async Task<IActionResult> PutInventario(string Codigo, [FromBody] InventarioDTO dto)
        { 
            return Ok(await context.PutInventario(Codigo, dto));
        }

        // POST: api/Inventarios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostInventario([FromBody] InventarioDTO dto)
        {
            return Ok(await context.PostInventario(dto));
        }

        // DELETE: api/Inventarios/5
        [HttpDelete("{Codigo}")]
        public async Task<IActionResult> DeleteInventario(string Codigo)
        {
            return Ok(await context.DeleteInventario(Codigo));
        }        
    }
}
