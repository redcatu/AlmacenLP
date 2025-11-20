using System.ComponentModel.DataAnnotations;

namespace AlmacenLP.Core.Entidades
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string UnidadMedida { get; set; }
        public string Marca { get; set; }
        public int Precio { get; set; }
        public string Estado { get; set; } = "Activo";

    }
}
