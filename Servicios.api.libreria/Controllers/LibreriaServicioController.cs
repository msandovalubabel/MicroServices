using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Servicios.api.libreria.Core.Entities;
using Servicios.api.libreria.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicios.api.libreria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibreriaServicioController : ControllerBase
    {
        private readonly IAutorRepository _autorRepository;
        private readonly IMongoRepository<AutorEntity> _autorGenericoRepository;
        private readonly IMongoRepository<EmpleadoEntity> _empleadoGenericoRepository;

        public LibreriaServicioController(IAutorRepository autorRepository, IMongoRepository<AutorEntity> autorGenericoRepository, IMongoRepository<EmpleadoEntity> empleadoGenericoRepository)
        {
            _autorRepository = autorRepository;
            _autorGenericoRepository = autorGenericoRepository;
            _empleadoGenericoRepository = empleadoGenericoRepository;
        }
        
        [HttpGet("autores")]
        public async Task<ActionResult<IEnumerable<Autor>>> GetAutores()
        {
            var autores = await _autorRepository.GetAutores();
            return Ok(autores);
        }

        //Para el genericos 
        [HttpGet("autorGenerico")]
        public async Task<ActionResult<IEnumerable<AutorEntity>>> GetAutorGenerico()
        {
            var autores = await _autorGenericoRepository.GetAll();
            return Ok(autores);
        }

        [HttpGet("empleadoGenerico")] /// Usar hasta despues de ver mongo docker
        public async Task<ActionResult<IEnumerable<EmpleadoEntity>>> GetEmpleadoGenerico()
        {
            var empleados = await _empleadoGenericoRepository.GetAll();
            return Ok(empleados);
        }
    }
}
