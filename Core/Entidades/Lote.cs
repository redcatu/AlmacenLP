using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlmacenLP.Core.Entidades
{
    public class Lote
    {
        [Key]
        public int Id { get; set; }
        public string CodigoProducto { get; set; }
        public string CodigoAlmacen { get; set; }
        public string Codigo { get; set; }
        public int Cantidad { get; set; }
        [Column(TypeName = "date")]
        public DateTime FechaIngreso { get; set; }
        [Column(TypeName = "date")]
        public DateTime FechaVencimiento { get; set; }
        public string Estado { get; set; } = "Activo";
    }
}
