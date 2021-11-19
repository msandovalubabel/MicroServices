using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Servicios.RabbitMQ.Bus.BusRabbit;
using Servicios.RabbitMQ.Bus.Comandos;
using Servicios.RabbitMQ.Bus.Eventos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Servicios.RabbitMQ.Bus.Implement
{
    public class RabbitEventBus : IRabbitEventBus
    {
        private readonly IMediator _mediator;
        private readonly Dictionary<string, List<Type>> _manejadores;
        private readonly List<Type> _eventosTipos;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public RabbitEventBus(IMediator mediator, IServiceScopeFactory serviceScopeFactory)
        {
            _mediator = mediator;
            _manejadores = new Dictionary<string, List<Type>>();
            _eventosTipos = new List<Type>();
            _serviceScopeFactory = serviceScopeFactory;
        }

        public Task EnviarComando<T>(T comando) where T : Comando
        {
            return _mediator.Send(comando);
        }

        public void Publish<T>(T evento) where T : Evento
        {
            var factory = new ConnectionFactory() { HostName = "172.24.158.33" };
            using(var connection=factory.CreateConnection())
            using (var channel= connection.CreateModel())
            {
                var eventName = evento.GetType().Name;
                channel.QueueDeclare(eventName, false, false, false, null); /// Es el nombre de la cola y la crea si no existe
                var message = JsonSerializer.Serialize(evento);
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish("", eventName, null, body);
            }
        }

        public void Suscribe<T, TH>()
            where T : Evento
            where TH : IEventoManejador<T>
        {
            var eventoNombre = typeof(T).Name;
            var manejadorEventoTipo = typeof(TH);
            //Agregamos en eventos tipo pero validamos
            if (!_eventosTipos.Contains(typeof(T)))
            {
                _eventosTipos.Add(typeof(T));
            }

            if (!_manejadores.ContainsKey(eventoNombre))
            {
                _manejadores.Add(eventoNombre, new List<Type>());
            }

            //Manejamos un control para arrojar una exception, si se registro con anterioridad
            if (_manejadores[eventoNombre].Any(x => x.GetType() == manejadorEventoTipo))
            {
                throw new ArgumentException($"El manejador {manejadorEventoTipo.Name} fue registrado anteriormente por {eventoNombre}");
            }

            _manejadores[eventoNombre].Add(manejadorEventoTipo);

            var factory = new ConnectionFactory() { HostName = "172.24.158.33", DispatchConsumersAsync=true };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            channel.QueueDeclare(eventoNombre, false, false, false, null);
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += Consumer_Delegate;
            channel.BasicConsume(eventoNombre, true, consumer);

        }

        private async Task Consumer_Delegate(object sender, BasicDeliverEventArgs e)
        {
            var nombreEvento = e.RoutingKey;
            var message = Encoding.UTF8.GetString(e.Body.ToArray());
            try
            {
                if (_manejadores.ContainsKey(nombreEvento))
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var subcriptions = _manejadores[nombreEvento];
                        foreach (var sb in subcriptions)
                        {
                            var manejador = scope.ServiceProvider.GetService(sb);
                            if (manejador == null) continue;
                            var tipoEvento = _eventosTipos.SingleOrDefault(x => x.Name == nombreEvento);
                            var eventoDS = JsonSerializer.Deserialize(message, tipoEvento);
                            var concretoTipo = typeof(IEventoManejador<>).MakeGenericType(tipoEvento);
                            await (Task)concretoTipo.GetMethod("Handle").Invoke(manejador, new object[] { eventoDS });
                        }
                    }
                
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
