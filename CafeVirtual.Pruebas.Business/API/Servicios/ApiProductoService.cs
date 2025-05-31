using Microsoft.Extensions.Options;
using CafeVirtual.Pruebas.Business.API.DTO;
using CafeVirtual.Pruebas.Business.API.Interfaces;
using CafeVirtual.Pruebas.Business.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace CafeVirtual.Pruebas.Business.API.Servicios
{
    public class ApiProductoService: IApiProductoService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public ApiProductoService(HttpClient httpClient, IOptions<ApiSettings> options)
        {
            _httpClient = httpClient;
            _baseUrl = options.Value.BaseUrl;
        }

        public async Task<ResultProductoListApiDTO?> ObtenerProducto(string token, string? busqueda)
        {
            var query = string.IsNullOrWhiteSpace(busqueda) ? "" : $"?Busqueda={busqueda}";
            string url = $"{_baseUrl}/Producto/obtenerProducto{query}";

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<ResultProductoListApiDTO>();

            return null;
        }      

        public async Task<ResultProductoApiDTO?> ObtenerProductoById(string token, int idProducto)
        {
            string url = $"{_baseUrl}/Producto/obtenerProductoById?idProducto={idProducto}";

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsync(url, null);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<ResultProductoApiDTO>();

            return null;
        }

        public async Task<ResultProductoApiDTO?> AgregarProducto(string token, ProductoDto producto)
        {
            string url = $"{_baseUrl}/Producto/agregarProducto";

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsJsonAsync(url, producto);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<ResultProductoApiDTO>();

            return null;
        }

        public async Task<ResultProductoApiDTO?> EditarProducto(string token, ProductoDto producto)
        {
            string url = $"{_baseUrl}/Producto/editarProducto";

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsJsonAsync(url, producto);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<ResultProductoApiDTO>();

            return null;
        }

        public async Task<ResultEliminarProductoApiDTO?> EliminarProducto(string token, int idProducto)
        {
            string url = $"{_baseUrl}/Producto/eliminarProducto?idProducto={idProducto}";

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsync(url, null);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<ResultEliminarProductoApiDTO>();

            return null;
        }
    }
}
