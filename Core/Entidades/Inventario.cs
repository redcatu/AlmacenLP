using System.ComponentModel.DataAnnotations;

namespace AlmacenLP.Core.Entidades
{
    public class Inventario
    {
        [Key]
        public int Id { get; set; }
        public string CodigoProducto { get; set; }
        public string CodigoAlmacen { get; set; }
        public string CodigoLote { get; set; }
        public string Codigo { get; set; }
        public int ProductoStock { get; set; }
        public string Estado { get; set; } = "Activo";
    }
}
