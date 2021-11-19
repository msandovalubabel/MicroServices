using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Servicios.api.libreria.Core.Entities;
using Servicios.api.libreria.Repository;
using Servicios.RabbitMQ.Bus.BusRabbit;
using Servicios.RabbitMQ.Bus.EventoQueue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicios.api.libreria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibroController : ControllerBase
    {
        private readonly IMongoRepository<LibroEntity> _libroRepository;
        private readonly IRabbitEventBus _eventBus;
        public LibroController(IMongoRepository<LibroEntity> libroRepository, IRabbitEventBus eventBus)
        {
            _libroRepository = libroRepository;
            _eventBus = eventBus;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LibroEntity>>> Get()
        {
            return Ok(await _libroRepository.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LibroEntity>> GetById(string Id)
        {
            var libro = await _libroRepository.GetById(Id);
            return Ok(libro);
        }

        [HttpPost]
        public async Task Post(LibroEntity libro)
        {
            await _libroRepository.InsertDocument(libro);
            _eventBus.Publish(new EmailEventoQueue("mauricio.sandoval@grupobabel.com","Api.Libro",$" Se creó el libro: {libro.Titulo}"));
        }

        [HttpPost("pagination")]
        public async Task<ActionResult<PaginationEntity<LibroEntity>>> PostPagintaion(PaginationEntity<LibroEntity> pagination)
        {
            var resultados = await _libroRepository.PaginationByFilter(pagination);
            return Ok(resultados);
        }

    }
}
