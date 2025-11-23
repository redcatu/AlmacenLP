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
            if (almacen.CantidadDisponible < almacen.CapacidadMaxima)
            {
                int ocupado = almacen.CapacidadMaxima - almacen.CantidadDisponible;
                throw new Exception($"No se puede eliminar el almacén porque contiene {ocupado} unidades de stock. Vacíe el almacén primero.");
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
            if (await context.Almacen.AnyAsync(a => a.Codigo == dto.Codigo))
            {
                throw new Exception($"El código de almacén '{dto.Codigo}' ya existe.");
            }

            if (dto.CapacidadMaxima <= 0)
            {
                throw new Exception("La capacidad máxima debe ser mayor a 0.");
            }
            var almacen = new Almacen
            {
                CodigoSucursal = dto.CodigoSucursal,
                Codigo = dto.Codigo,
                Nombre = dto.Nombre,
                CapacidadMaxima = dto.CapacidadMaxima,
                CantidadDisponible = dto.CapacidadMaxima
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
            if (almacen.CapacidadMaxima != dto.CapacidadMaxima)
            {
                int espacioOcupado = almacen.CapacidadMaxima - almacen.CantidadDisponible;
                if (dto.CapacidadMaxima < espacioOcupado)
                {
                    throw new Exception($"No se puede reducir la capacidad a {dto.CapacidadMaxima} porque actualmente hay {espacioOcupado} unidades ocupando espacio.");
                }
                almacen.CantidadDisponible = dto.CapacidadMaxima - espacioOcupado;
            }
            almacen.CodigoSucursal = dto.CodigoSucursal;
            
            almacen.Nombre = dto.Nombre;
            almacen.CapacidadMaxima = dto.CapacidadMaxima;
            
            context.Almacen.Update(almacen);
            await context.SaveChangesAsync();
            return almacen.toAlmacenDTO();
        }
    }
}
