﻿using MongoDB.Driver;
using Servicios.api.libreria.Core.ContexMongoDB;
using Servicios.api.libreria.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicios.api.libreria.Repository
{
    public class AutorRepository : IAutorRepository
    {
        private readonly IAutorContext _autorContext;
        public AutorRepository(IAutorContext autorContext)
        {
            _autorContext = autorContext;
        }
        public async Task<IEnumerable<Autor>> GetAutores()
        {
           return await _autorContext.Autores.Find(p => true).ToListAsync();
        }
    }
}
