using Servicios.api.libreria.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Servicios.api.libreria.Repository
{
    public interface IMongoRepository<TDocument> where TDocument: IDocument
    {
        Task<IEnumerable<TDocument>> GetAll();

        /// <summary>
        /// Llenar hasta el final 
        /// </summary>
        
        Task<TDocument> GetById(string Id);
        Task InsertDocument(TDocument document);
        Task UpdateDocument(TDocument document);
        Task DeleteById(string Id);
        //Paginación
        Task<PaginationEntity<TDocument>> PaginationBy(Expression<Func<TDocument,bool>> filterExpression,PaginationEntity<TDocument>pagination);
        Task<PaginationEntity<TDocument>> PaginationByFilter(PaginationEntity<TDocument> pagination);
    }
}
