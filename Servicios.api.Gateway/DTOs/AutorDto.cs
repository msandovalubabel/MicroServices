using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicios.api.Gateway.DTOs
{
    public class AutorDto:AutorRemote
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
    }
}
