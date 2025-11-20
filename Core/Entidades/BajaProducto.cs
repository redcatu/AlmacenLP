using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlmacenLP.Core.Entidades
{
    public class BajaProducto
    {
        [Key]
        public int Id { get; set; }
        public int IdProducto { get; set; }
        public int IdAlmacen { get; set; }
        public string Codigo { get; set; }
        public int Cantidad { get; set; }
        public string TipoBaja { get; set; }
        [Column(TypeName = "date")]
        public DateTime FechaBaja { get; set; }
        public string Estado { get; set; } = "Activo";
    }
}

