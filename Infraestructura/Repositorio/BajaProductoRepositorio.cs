using AlmacenLP.Core.DTOs;
using AlmacenLP.Core.Entidades;
using AlmacenLP.Core.Interfaces;
using AlmacenLP.Core.Mapeadores;
using AlmacenLP.Infraestructura.Data;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AlmacenLP.Infraestructura.Repositorio
{
    public class BajaProductoRepositorio : IBajaProductoRepositorio
    {
        private readonly AlmacenLPContext context;

        public BajaProductoRepositorio(AlmacenLPContext context)
        {
            this.context = context;
        }

        public async Task<BajaProductoDTO> DeleteBajaProducto(string Codigo)
        {
            var bajaProducto = await context.BajaProducto.FirstOrDefaultAsync(p => p.Codigo == Codigo);
            if (bajaProducto == null)
            {
                throw new Exception("Dato no encontrado");
            }
            bajaProducto.Estado = "Inactivo";
            context.BajaProducto.Update(bajaProducto);
            await context.SaveChangesAsync();

            return bajaProducto.toBajaProductoDTO();
        }
    
        public async Task<List<BajaProductoDTO>> GetBajaProducto()
        {
            var bajaProducto = await (from c in context.BajaProducto
                                 where c.Estado != "Inactivo"
                                 select c
                         ).Select(ca => ca.toBajaProductoDTO()).ToListAsync();
            return bajaProducto;
        }

        public async Task<BajaProductoDTO> GetBajaProducto(string Codigo)
        {
            return await (from c in context.BajaProducto
                          where c.Codigo == Codigo
                          select c.toBajaProductoDTO()).FirstOrDefaultAsync();
        }

        public async Task<BajaProductoDTO> PostBajaProducto(int IdProducto, int IdAlmacen, string Codigo, int Cantidad, string TipoBaja, DateTime FechaBaja)
        {
            var bajaProducto = new BajaProducto
            {
                IdProducto = IdProducto,
                IdAlmacen = IdAlmacen,
                Codigo = Codigo,
                Cantidad = Cantidad,
                TipoBaja = TipoBaja,
                FechaBaja = FechaBaja

            };
            context.BajaProducto.Add(bajaProducto);
            await context.SaveChangesAsync();
            return bajaProducto.toBajaProductoDTO();
        }

        public async Task<BajaProductoDTO> PutBajaProducto(string Codigo, int NuevoIdProducto, int NuevoIdAlmacen, string NuevoCodigo, int NuevaCantidad, string NuevoTipoBaja, DateTime NuevaFechaBaja)
        {
            var bajaProducto = await context.BajaProducto.FirstOrDefaultAsync(c => c.Codigo == Codigo);
            if (bajaProducto == null)
            {
                throw new Exception("Datos no encontrados");
            }
            bajaProducto.IdProducto = NuevoIdProducto;
            bajaProducto.IdAlmacen = NuevoIdProducto;
            bajaProducto.Codigo = NuevoCodigo;
            bajaProducto.Cantidad = NuevaCantidad;
            bajaProducto.TipoBaja = NuevoTipoBaja;
            bajaProducto.FechaBaja = NuevaFechaBaja;
            context.BajaProducto.Update(bajaProducto);
            await context.SaveChangesAsync();
            return bajaProducto.toBajaProductoDTO();
        }
    }
}
