using Microsoft.Extensions.Logging;
using Servicios.api.Gateway.DTOs;
using Servicios.api.Gateway.RemoteInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Servicios.api.Gateway.RemoteImplement
{
    public class AutorRemote : IAutorRemote
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly ILogger<AutorRemote> _logger;

        public AutorRemote(IHttpClientFactory httpClient, ILogger<AutorRemote> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        public async Task<(bool resultado, AutorDto autor, string ErrorMessage)> GetAutor(string Id)
        {
            try
            {
                var cliente = _httpClient.CreateClient("AutorService");
                var response = await cliente.GetAsync($"/api/LibreriaAutor/{Id}");
                if (response.IsSuccessStatusCode)
                {
                    var contenido = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                    var resultado = JsonSerializer.Deserialize<DTOs.AutorDto>(contenido, options);
                    return (true, resultado, null);
                }
                return (false, null, response.ReasonPhrase);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return (false, null, e.Message);
            }
            
            
            
        }
    }
}
