using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicios.api.Gateway.DTOs
{
    public class LibroRemote
    {
        public string Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public int Precio { get; set; }
        public DateTime FechaPublicacion { get; set; }
        public AutorRemote Auto { get; set; }
        public AutorDto Autor { get; set; }

    }
}
