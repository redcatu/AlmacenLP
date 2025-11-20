using System.ComponentModel.DataAnnotations.Schema;

namespace AlmacenLP.Core.DTOs
{
    public class LoteDTO
    {
        public int IdProducto { get; set; }
        public int IdAlmacen { get; set; }
        public string Codigo { get; set; }
        public int Cantidad { get; set; }
        [Column(TypeName = "date")]
        public DateTime FechaIngreso { get; set; }
        [Column(TypeName = "date")]
        public DateTime FechaVencimiento { get; set; }
    }
}
