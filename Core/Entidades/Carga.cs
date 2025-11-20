using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlmacenLP.Core.Entidades
{
    public class Carga
    {
        [Key]
        public int Id { get; set; }
        public int IdProducto { get; set; }
        public int IdCamion {  get; set; }
        public int IdAlmacen { get; set; }
        public int IdVenta { get; set; }
        public string Codigo { get; set; }
        public int Cantidad { get; set; }
        public string TipoMovimiento { get; set; }
        [Column(TypeName = "date")]
        public DateTime FechaSalida { get; set; }
        public string Estado { get; set; } = "Pendiente";
    }
}
