using CafeVirtual.Pruebas.Business.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using CafeVirtual.Pruebas.Business.API.Models;
using CafeVirtual.Pruebas.Business.DTO;
using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using CafeVirtual.Pruebas.Business.Settings;
using Microsoft.Extensions.Configuration;

namespace CafeVirtual.Pruebas.Business.API.Servicios
{
    public class ApiAuthService: IApiAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public ApiAuthService(HttpClient httpClient, IOptions<ApiSettings> options)
        {
            _httpClient = httpClient;
            _baseUrl = options.Value.BaseUrl;            
        }

        public async Task<LoginResponse?> ObtenerTokenAsync(string email, string password)
        {
            var loginPayload = new { email, password };
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/Login/Login", loginPayload);            

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<LoginResponse>();
                return content;
            }

            return null;
        }

        public async Task<LoginDTO?> ObtenerUsuarioConTokenAsync(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync("https://tu-api.com/api/usuarios/perfil");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<LoginDTO>();
            }

            return null;
        }
    }
}
