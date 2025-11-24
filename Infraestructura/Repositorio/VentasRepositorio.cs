using AlmacenLP.Core.DTOs;
using AlmacenLP.Core.Interfaces;
using System.Text.Json;

namespace AlmacenLP.Infraestructura.Repositorio
{
    public class VentasRepositorio : IVentasRepositorio
    {
        private readonly HttpClient httpClient;
        public VentasRepositorio(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<List<VentaDTO>> GetProductoCantidad()
        {
            string URL = "https://ventassc-production.up.railway.app/api/DetallePedidos/ProductoCantidad";
            HttpResponseMessage respuesta = await httpClient.GetAsync(URL);

            if (!respuesta.IsSuccessStatusCode)
            {
                throw new Exception("Error al obtener datos");
            }
            string jsonRespuesta = await respuesta.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var datos = JsonSerializer.Deserialize<List<VentaDTO>>(jsonRespuesta, options);

            return datos;
        }
    }
}
