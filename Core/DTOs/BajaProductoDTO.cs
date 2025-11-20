using System.ComponentModel.DataAnnotations.Schema;

namespace AlmacenLP.Core.DTOs
{
    public class BajaProductoDTO
    {
        public int IdProducto { get; set; }
        public int IdAlmacen { get; set; }
        public string Codigo { get; set; }
        public int Cantidad { get; set; }
        public string TipoBaja { get; set; }
        [Column(TypeName = "date")]
        public DateTime FechaBaja { get; set; }
    }
}
