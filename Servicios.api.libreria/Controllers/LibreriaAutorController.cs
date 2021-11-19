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
    public class LibreriaAutorController : ControllerBase
    {
        private readonly IMongoRepository<AutorEntity> _autorRepository;
        public LibreriaAutorController(IMongoRepository<AutorEntity> autorRepository)
        {
            _autorRepository = autorRepository;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AutorEntity>>> Get()
        {
            var autores = await _autorRepository.GetAll();
            return Ok(autores);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AutorEntity>> GetById(string id)
        {
            var autor = await _autorRepository.GetById(id);
            return Ok(autor);
        }

        [HttpPut("{id}")]
        public async Task Put(string id,AutorEntity autor)
        {
            autor.Id = id;
            await _autorRepository.UpdateDocument(autor);
        }

        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
           await _autorRepository.DeleteById(id);
        }

        //[HttpPost("Pagination")]
        //public async Task<ActionResult<PaginationEntity<AutorEntity>>> PostPagination(PaginationEntity<AutorEntity> pagination)
        //{
        //    var resultados = await _autorRepository.PaginationBy(
        //        filter=>filter.Nombre==pagination.Filter,pagination                
        //        );
        //    return Ok(resultados);
        //}

        [HttpPost("Pagination")]
        public async Task<ActionResult<PaginationEntity<AutorEntity>>> PostPaginationFilter(PaginationEntity<AutorEntity> pagination)
        {
            var resultados = await _autorRepository.PaginationByFilter(pagination);
            return Ok(resultados);
        }

    }
}
