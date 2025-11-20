using AlmacenLP.Core.DTOs;
using AlmacenLP.Core.Entidades;
using AlmacenLP.Core.Interfaces;
using AlmacenLP.Core.Mapeadores;
using AlmacenLP.Infraestructura.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlmacenLP.Infraestructura.Repositorio
{
    public class CamionRepositorio : ICamionRepositorio
    {
        private readonly AlmacenLPContext context;
        public CamionRepositorio(AlmacenLPContext context)
        {
            this.context = context;
        }

        public async Task<CamionDTO> DeleteCamion(string Placa)
        {
            var camion = await context.Camion.FirstOrDefaultAsync(p => p.Placa == Placa);
            if (camion == null)
            {
                throw new Exception("Camion no encontrado");
            }
            camion.Estado = "No disponible";
            context.Camion.Update(camion);
            await context.SaveChangesAsync();

            return camion.toCamionDTO();
        }
        public bool CamionExists(string Placa)
        {
            return context.Camion.Any(p => p.Placa == Placa);
        }

        public async Task<CamionDTO> GetCamion(string Placa)
        {
            return await(from c in context.Camion
                         where c.Placa == Placa
                         select c.toCamionDTO()).FirstOrDefaultAsync();
        }

        public async Task<List<CamionDTO>> GetCamiones()
        {
            var camion = await (from c in context.Camion
                          where c.Estado != "No disponible"
                          select c
                         ).Select(ca => ca.toCamionDTO()).ToListAsync();
            return camion;
        }

        public async Task<CamionDTO> PostCamion(string Placa, string Modelo, int CapacidadMaxima, int CantidadDisponible)
        {
            var camion = new Camion
            {
                Placa = Placa,
                Modelo = Modelo,
                CapacidadMaxima = CapacidadMaxima,
                CantidadDisponible = CantidadDisponible
            };
            context.Camion.Add(camion);
            await context.SaveChangesAsync();
            return camion.toCamionDTO();
        }

        public async Task<CamionDTO> PutCamion(string Placa, string NuevaPlaca, string NuevoModelo, int NuevaCapacidadMaxima, int NuevaCantidadDisponible)
        {
            var camion = await context.Camion.FirstOrDefaultAsync(c => c.Placa == Placa);
            if (camion == null)
            {
                throw new Exception("Producto no encontrado");
            }
            camion.Placa = NuevaPlaca;
            camion.Modelo = NuevoModelo;
            camion.CapacidadMaxima = NuevaCapacidadMaxima;
            camion.CantidadDisponible = NuevaCantidadDisponible;

            context.Camion.Update(camion);
            await context.SaveChangesAsync();
            return camion.toCamionDTO();
        }

    }
}
