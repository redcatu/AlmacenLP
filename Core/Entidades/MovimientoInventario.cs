using System.ComponentModel.DataAnnotations;

namespace AlmacenLP.Core.Entidades
{
    public class MovimientoInventario
    {
        [Key]
        public int Id { get; set; }
        public string CodigoProducto { get; set; }
        public string? CodigoCamion { get; set; }
        public string CodigoAlmacen { get; set; }
        public string? CodigoVenta {  get; set; }
        public string? CodigoLote { get; set; }
        public string Codigo {  get; set; }
        public int? CantidadBuena { get; set; }
        public int? CantidadMala { get; set; }
        public string TipoMovimiento { get; set; }
        public string? Motivo { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; } = "Activo";
        
    }
}
