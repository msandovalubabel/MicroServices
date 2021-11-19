using Microsoft.Extensions.Logging;
using Servicios.RabbitMQ.Bus.BusRabbit;
using Servicios.RabbitMQ.Bus.EventoQueue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicios.api.Mail.ManejadorRabbit
{
    public class EmailEventoManejador : IEventoManejador<EmailEventoQueue>
    {
        private readonly ILogger<EmailEventoManejador> _logger;
        public EmailEventoManejador()
        {

        }
        public EmailEventoManejador(ILogger<EmailEventoManejador> logger)
        {
            _logger = logger;
        }
        public Task Handle(EmailEventoQueue @event)
        {
            _logger.LogInformation($"La Información contenida: Destinatario{@event.Destinatario}, Contenido:{@event.Contenido}");

            return Task.CompletedTask;
        }
    }
}
