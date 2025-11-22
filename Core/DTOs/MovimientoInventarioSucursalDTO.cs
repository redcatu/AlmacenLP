namespace AlmacenLP.Core.DTOs
{
    public class MovimientoInventarioSucursalDTO
    {
        public string CodigoProducto { get; set; }
        public string Codigo { get; set; }
        public int? CantidadBuena { get; set; }
        public int? CantidadMala { get; set; }
        public string TipoMovimiento { get; set; }
        public string? Motivo { get; set; }
        public DateTime Fecha { get; set; }
    }
}
