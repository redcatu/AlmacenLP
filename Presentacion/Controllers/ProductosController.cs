using AlmacenLP.Core.DTOs;
using AlmacenLP.Core.Entidades;
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
    public class ProductosController : ControllerBase
    {
        private readonly IProductoRepositorio context;

        public ProductosController(IProductoRepositorio context)
        {
            this.context = context;
        }
        
        // GET: api/Productos
        [HttpGet("Lista")]
        public async Task<IActionResult> GetProductos()
        {
            return Ok(await context.GetProductos());
        }
        // GET: api/Productos
        [HttpGet("Ventas")]
        public async Task<IActionResult> GetProductoVentas()
        {
            return Ok(await context.GetProductoVentas());
        }
        // GET: api/Productos
        [HttpGet("Lista Borrados")]
        public async Task<IActionResult> GetProductosBorrados()
        {
            return Ok(await context.GetProductosBorrados());
        }

        // GET: api/Productos/5
        [HttpGet("{codigo}")]
        public async Task<IActionResult> GetProducto(string codigo)
        {
            ProductoDTO producto = await context.GetProducto(codigo);

            if (producto == null)
            {
                return NotFound();
            }

            return Ok(producto);
        }
        
        // PUT: api/Productos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{codigo}")]
        public async Task<IActionResult> PutProducto(string codigo, [FromBody] ProductoDTO dto )
        {
            ProductoDTO producto = await context.PutProducto(codigo, dto);
            if (producto == null)
            {
                return NotFound();
            }
            return Ok(producto);
        }
        
        // POST: api/Productos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostProducto([FromBody] ProductoDTO producto)
        {

            return Ok(await context.PostProducto(producto));
        }
        
        // DELETE: api/Productos/5
        [HttpDelete("{codigo}")]
        public async Task<IActionResult> DeleteProducto(string codigo)
        {
            return Ok(await context.DeleteProducto(codigo));
        }

        private bool ProductoExists(string codigo)
        {
            return ProductoExists(codigo);
        }
        
    }
}
