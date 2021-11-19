﻿using Servicios.RabbitMQ.Bus.Eventos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios.RabbitMQ.Bus.BusRabbit
{
    public interface IEventoManejador<in TEvent>:IEventoManejador where TEvent:Evento
    {
        Task Handle(TEvent @event);
    }
    public interface IEventoManejador
    {

    }

}
