using Servicios.api.Gateway.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicios.api.Gateway.RemoteInterface
{
    public interface IAutorRemote
    {
        Task<(bool resultado, AutorDto autor, string ErrorMessage)> GetAutor(string Id);

    }
}
