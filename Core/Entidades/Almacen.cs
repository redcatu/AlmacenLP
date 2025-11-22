using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AlmacenLP.Core.Entidades
{
    public class Almacen
    {
        [Key]
        public int Id { get; set; }
        public string CodigoSucursal { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public int CapacidadMaxima { get; set; }
        public int CantidadDisponible { get; set; }
        public string Estado { get; set; } = "Activo";
    }
}
