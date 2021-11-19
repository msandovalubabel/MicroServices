using Servicios.RabbitMQ.Bus.Eventos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios.RabbitMQ.Bus.Comandos
{
    public abstract class Comando:Message
    {
        public DateTime Timestamp { get; protected set; }

        protected Comando()
        {
            Timestamp = DateTime.Now;
        }
    }
}
