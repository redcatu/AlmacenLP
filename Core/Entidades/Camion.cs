using System.ComponentModel.DataAnnotations;

namespace AlmacenLP.Core.Entidades
{
    public class Camion
    {
        [Key]
        public int Id { get; set; }
        public string Placa { get; set; }
        public string Modelo { get; set; }
        public int CapacidadMaxima { get; set; }
        public int CantidadDisponible { get; set; }
        public string Estado { get; set; } = "Disponible";
    }
}
