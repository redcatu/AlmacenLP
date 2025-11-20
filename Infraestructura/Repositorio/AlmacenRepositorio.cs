using AlmacenLP.Core.DTOs;
using AlmacenLP.Core.Entidades;
using AlmacenLP.Core.Interfaces;
using AlmacenLP.Core.Mapeadores;
using AlmacenLP.Infraestructura.Data;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace AlmacenLP.Infraestructura.Repositorio
{
    public class AlmacenRepositorio : IAlmacenRepositorio
    {
        private readonly AlmacenLPContext context;
        public AlmacenRepositorio(AlmacenLPContext context)
        {
            this.context = context;
        }
        public async Task<AlmacenDTO> DeleteAlmacen(string codigo)
        {
            var almacen = await context.Almacen.FirstOrDefaultAsync(p => p.Codigo == codigo);
            if (almacen == null)
            {
                throw new Exception("Almacen no encontrado");
            }
            almacen.Estado = "Inactivo";
            context.Almacen.Update(almacen);
            await context.SaveChangesAsync();

            return almacen.toAlmacenDTO();
        }

        public async Task<List<AlmacenDTO>> GetAlmacen()
        {
            var almacen = await (from c in context.Almacen
                                where c.Estado != "Inactivo"
                                select c
                         ).Select(ca => ca.toAlmacenDTO()).ToListAsync();
            return almacen;
        }

        public async Task<AlmacenDTO> GetAlmacen(string Codigo)
        {
            return await (from c in context.Almacen
                          where c.Codigo == Codigo
                          select c.toAlmacenDTO()).FirstOrDefaultAsync();
        }

        public async Task<AlmacenDTO> PostAlmacen(int IdSucursal, string Codigo, string Nombre, int CapacidadMaxima, int CantidadDisponible)
        {
            var almacen = new Almacen
            {
                IdSucursal = IdSucursal,
                Codigo = Codigo,
                Nombre = Nombre,
                CapacidadMaxima = CapacidadMaxima,
                CantidadDisponible = CantidadDisponible
            };
            context.Almacen.Add(almacen);
            await context.SaveChangesAsync();
            return almacen.toAlmacenDTO();
        }

        public async Task<AlmacenDTO> PutAlmacen(string Codigo, int NuevoIdSucursal, string NuevoCodigo, string NuevoNombre, int NuevaCapacidadMaxima, int NuevaCantidadDisponible)
        {
            var almacen = await context.Almacen.FirstOrDefaultAsync(c => c.Codigo == Codigo);
            if (almacen == null)
            {
                throw new Exception("Almacen no encontrado");
            }
            almacen.IdSucursal = NuevoIdSucursal;
            almacen.Codigo = NuevoCodigo;
            almacen.Nombre = NuevoNombre;
            almacen.CapacidadMaxima = NuevaCapacidadMaxima;
            almacen.CantidadDisponible = NuevaCantidadDisponible;
            context.Almacen.Update(almacen);
            await context.SaveChangesAsync();
            return almacen.toAlmacenDTO();
        }
    }
}
