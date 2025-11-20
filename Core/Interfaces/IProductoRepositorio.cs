using AlmacenLP.Core.DTOs;
using AlmacenLP.Core.Entidades;
using Microsoft.AspNetCore.Mvc;
namespace AlmacenLP.Core.Interfaces
{
    public interface IProductoRepositorio
    {
        //La utilidad de esta funcion es encontrar un producto a traves de su codigo.
        Task<ProductoDTO> GetProducto(string codigo);
        //La utilidad de esta funcion es obtener una lista de todos los productos con el estado activo.
        Task<List<ProductoDTO>> GetProductos();
        //La utilidad de esta funcion es obtener una lista de todos los productos borrados con el estado inactivos.
        Task<List<ProductoDTO>> GetProductosBorrados();
        //La utilidad de esta funcion es crear un nuevo producto.
        Task<ProductoDTO> PostProducto([FromBody] ProductoDTO producto);
        //La utilidad de esta funcion es eliminar un producto a traves del cambio de estado del mismo.
        Task<ProductoDTO> DeleteProducto(string codigo);
        //La utilidad de esta funcion es cambiar los valores de los atributos de un producto.
        Task<ProductoDTO> PutProducto(string codigo, [FromBody] ProductoDTO dto);
        


    }
}
