using AlmacenLP.Core.DTOs;
using AlmacenLP.Core.Interfaces;
using AlmacenLP.Core.Mapeadores;
using AlmacenLP.Infraestructura.Data;
using Microsoft.EntityFrameworkCore;
using AlmacenLP.Core.Entidades;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AlmacenLP.Infraestructura.Repositorio
{
    public class CargaRepositorio : ICargaRepositorio
    {
        private readonly AlmacenLPContext context;
        public CargaRepositorio(AlmacenLPContext context)
        {
            this.context = context;
        }

        public async Task<CargaDTO> DeleteCarga(string Codigo)
        {
            var carga = await context.Carga.FirstOrDefaultAsync(c => c.Codigo == Codigo);
            if (carga == null)
            {
                throw new Exception("Carga no encontrada");
            }
            carga.Estado = "Borrado";
            context.Carga.Update(carga);
            await context.SaveChangesAsync();

            return carga.toCargaDTO();
        }

        public async Task<List<CargaDTO>> GetCarga()
        {
            var carga = await (from c in context.Carga
                               where c.Estado != "Borrado"
                               select c
                              ).Select(ca => ca.toCargaDTO()).ToListAsync();
            return carga;
        }
        public async Task<CargaDTO> PostCarga(int IdProducto, int IdCamion, int IdAlmacen, int IdVenta, string Codigo, int Cantidad, string TipoMovimiento, DateTime FechaSalida)
        {
            var carga = new Carga
            {
                IdProducto = IdProducto,
                IdCamion = IdCamion,
                IdAlmacen = IdAlmacen,
                IdVenta = IdVenta,
                Codigo = Codigo,
                Cantidad = Cantidad,
                TipoMovimiento = TipoMovimiento,
                FechaSalida = FechaSalida
            };
            context.Carga.Add(carga);
            await context.SaveChangesAsync();
            return carga.toCargaDTO();
        }
        public async Task<CargaDTO> PutCarga(string Codigo, int IdProductoNuevo, int IdCamionNuevo, int IdAlmacenNuevo, int IdVentaNueva, string CodigoNuevo, int CantidadNueva, string TipoMovimientoNuevo, DateTime FechaSalidaNueva)
        {
            var carga = await context.Carga.FirstOrDefaultAsync(c=>c.Codigo==Codigo);

            if(carga == null)
            {
                throw new Exception("Carga no encontrada");
            }
            carga.IdProducto = IdProductoNuevo;
            carga.IdCamion = IdCamionNuevo;
            carga.IdAlmacen = IdAlmacenNuevo;
            carga.IdVenta = IdVentaNueva;
            carga.Codigo = CodigoNuevo;
            carga.Cantidad = CantidadNueva;
            carga.TipoMovimiento = TipoMovimientoNuevo;
            carga.FechaSalida = FechaSalidaNueva;

            context.Carga.Update(carga);
            await context.SaveChangesAsync();
            return carga.toCargaDTO();
        }

    }
}
