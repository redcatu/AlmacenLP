using System.ComponentModel.DataAnnotations.Schema;

namespace AlmacenLP.Core.DTOs
{
    public class CargaDTO
    {
        public int IdProducto { get; set; }
        public int IdCamion { get; set; }
        public int IdAlmacen { get; set; }
        public int IdVenta { get; set; }
        public string Codigo { get; set; }
        public int Cantidad { get; set; }
        public string TipoMovimiento { get; set; }
        [Column(TypeName = "date")]
        public DateTime FechaSalida { get; set; }
    }
}
