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
    public class CamionesController : ControllerBase
    {
        private readonly ICamionRepositorio context;

        public CamionesController(ICamionRepositorio context)
        {
            this.context = context;
        }
        
        // GET: api/Camiones
        [HttpGet]
        public async Task<IActionResult> GetCamiones()
        {
            return Ok(await context.GetCamiones());
        }
        // POST: api/Camiones
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostCamion(string Placa, string Modelo, int CapacidadMaxima, int CantidadDisponible)
        {
            return Ok(await context.PostCamion(Placa, Modelo, CapacidadMaxima, CantidadDisponible));
        }
        // GET: api/Camiones/5
        [HttpGet("{Placa}")]
        public async Task<IActionResult> GetCamion(string Placa)
        {
            return Ok(await context.GetCamion(Placa));
        }
        // PUT: api/Camiones/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{Placa}")]
        public async Task<IActionResult> PutCamion(string Placa, string NuevaPlaca, string NuevoModelo, int NuevaCapacidadMaxima, int NuevaCantidadDisponible)
        {
            return Ok(await context.PutCamion(Placa,NuevaPlaca,NuevoModelo, NuevaCapacidadMaxima, NuevaCantidadDisponible));
        }
        // DELETE: api/Camiones/5
        [HttpDelete("{Placa}")]
        public async Task<IActionResult> DeleteCamion(string Placa)
        {
            return Ok(await context.DeleteCamion(Placa));
        }
        private bool CamionExists(string Placa)
        {
            return CamionExists(Placa);
        }
    }
}
