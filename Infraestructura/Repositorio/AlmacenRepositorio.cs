using AlmacenLP.Core.DTOs;
using AlmacenLP.Core.Entidades;
using AlmacenLP.Core.Interfaces;
using AlmacenLP.Core.Mapeadores;
using AlmacenLP.Infraestructura.Data;
using Microsoft.AspNetCore.Mvc;
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
            almacen.Estado = "Borrado";
            context.Almacen.Update(almacen);
            await context.SaveChangesAsync();

            return almacen.toAlmacenDTO();
        }

        public async Task<List<AlmacenDTO>> GetAlmacen()
        {
            var almacen = await (from c in context.Almacen
                                where c.Estado != "Borrado"
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

        public async Task<AlmacenDTO> PostAlmacen([FromBody] AlmacenDTO dto)
        {
            var almacen = new Almacen
            {
                CodigoSucursal = dto.CodigoSucursal,
                Codigo = dto.Codigo,
                Nombre = dto.Nombre,
                CapacidadMaxima = dto.CapacidadMaxima,
                CantidadDisponible = dto.CantidadDisponible
            };
            context.Almacen.Add(almacen);
            await context.SaveChangesAsync();
            return almacen.toAlmacenDTO();
        }

        public async Task<AlmacenDTO> PutAlmacen(string Codigo, [FromBody] AlmacenDTO dto)
        {
            var almacen = await context.Almacen.FirstOrDefaultAsync(c => c.Codigo == Codigo);
            if (almacen == null)
            {
                throw new Exception("Almacen no encontrado");
            }
            almacen.CodigoSucursal = dto.CodigoSucursal;
            almacen.Codigo = dto.Codigo;
            almacen.Nombre = dto.Nombre;
            almacen.CapacidadMaxima = dto.CapacidadMaxima;
            almacen.CantidadDisponible = dto.CantidadDisponible;
            context.Almacen.Update(almacen);
            await context.SaveChangesAsync();
            return almacen.toAlmacenDTO();
        }
    }
}
