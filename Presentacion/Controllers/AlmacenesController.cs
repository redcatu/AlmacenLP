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
    public class AlmacenesController : ControllerBase
    {
        private readonly IAlmacenRepositorio context;

        public AlmacenesController(IAlmacenRepositorio context)
        {
            this.context = context;
        }

        // GET: api/Almacenes
        [HttpGet]
        public async Task<IActionResult> GetAlmacen()
        {
            return Ok(await context.GetAlmacen());
        }

        // GET: api/Almacenes/5
        [HttpGet("{Codigo}")]
        public async Task<IActionResult> GetAlmacen(string Codigo)
        {
            return Ok(await context.GetAlmacen(Codigo));
        }

        // PUT: api/Almacenes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{Codigo}")]
        public async Task<IActionResult> PutAlmacen(string Codigo, int NuevoIdSucursal, string NuevoCodigo, string NuevoNombre, int NuevaCapacidadMaxima, int NuevaCantidadDisponible)
        {
            return Ok(await context.PutAlmacen(Codigo, NuevoIdSucursal, NuevoCodigo, NuevoNombre, NuevaCapacidadMaxima, NuevaCantidadDisponible));
        }

        // POST: api/Almacenes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostAlmacen(int IdSucursal, string Codigo, string Nombre, int CapacidadMaxima, int CantidadDisponible)
        {
            return Ok(await context.PostAlmacen(IdSucursal, Codigo, Nombre, CapacidadMaxima, CantidadDisponible));
        }

        // DELETE: api/Almacenes/5
        [HttpDelete("{codigo}")]
        public async Task<IActionResult> DeleteAlmacen(string codigo)
        {
            return Ok(await context.DeleteAlmacen(codigo));
        }
    }
}
