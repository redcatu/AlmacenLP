namespace AlmacenLP.Core.DTOs
{
    public class InventarioDTO
    {
        public int IdProducto { get; set; }
        public int IdAlmacen { get; set; }
        public string Codigo { get; set; }
        public int ProductoStock { get; set; }
    }
}
