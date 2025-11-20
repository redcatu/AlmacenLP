using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AlmacenLP.Core.Entidades;

namespace AlmacenLP.Infraestructura.Data
{
    public class AlmacenLPContext : DbContext
    {
        public AlmacenLPContext (DbContextOptions<AlmacenLPContext> options)
            : base(options)
        {
        }

        public DbSet<Almacen> Almacen { get; set; } = default!;
        public DbSet<BajaProducto> BajaProducto { get; set; } = default!;
        public DbSet<Camion> Camion { get; set; } = default!;
        public DbSet<Carga> Carga { get; set; } = default!;
        public DbSet<Inventario> Inventario { get; set; } = default!;
        public DbSet<Lote> Lote { get; set; } = default!;
        public DbSet<Producto> Producto { get; set; } = default!;
    }
}
