using Servicios.RabbitMQ.Bus.Comandos;
using Servicios.RabbitMQ.Bus.Eventos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios.RabbitMQ.Bus.BusRabbit
{
    public interface IRabbitEventBus
    {
        Task EnviarComando<T>(T comando) where T : Comando;
        void Publish<T>(T @evento) where T : Evento;

        void Suscribe<T, TH>() where T : Evento
            where TH : IEventoManejador<T>;
    }
}
