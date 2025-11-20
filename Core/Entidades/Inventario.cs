using System.ComponentModel.DataAnnotations;

namespace AlmacenLP.Core.Entidades
{
    public class Inventario
    {
        [Key]
        public int Id { get; set; }
        public int IdProducto { get; set; }
        public int IdAlmacen { get; set; }
        public string Codigo { get; set; }
        public int ProductoStock { get; set; }
        public string Estado { get; set; } = "Disponible";
    }
}
