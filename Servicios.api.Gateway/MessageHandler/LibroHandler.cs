using Microsoft.Extensions.Logging;
using Servicios.api.Gateway.DTOs;
using Servicios.api.Gateway.RemoteInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Servicios.api.Gateway.MessageHandler
{
    public class LibroHandler : DelegatingHandler
    {
        private readonly ILogger<LibroHandler> _logger;
        private readonly IAutorRemote _autorRemote;

        public LibroHandler(ILogger<LibroHandler> logger, IAutorRemote autorRemote)
        {
            _logger = logger;
            _autorRemote = autorRemote;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var respuesta = await base.SendAsync(request, cancellationToken);
            if (respuesta.IsSuccessStatusCode)
            {
                var contenido = await respuesta.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var resultado = JsonSerializer.Deserialize<LibroRemote>(contenido, options);
                var responseAutor = await _autorRemote.GetAutor(resultado.Autor.Id);

                if(responseAutor.resultado)
                {
                    resultado.Auto = responseAutor.autor;
                    var resultadoString = JsonSerializer.Serialize(resultado);
                    respuesta.Content = new StringContent(resultadoString, System.Text.Encoding.UTF8, "application/json");
                }
            }
            return respuesta;
        }
    }
}
