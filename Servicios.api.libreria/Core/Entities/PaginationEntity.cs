using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicios.api.libreria.Core.Entities
{
    public class PaginationEntity<TDocment>
    {
        public int PageSizee { get; set; }
        public int Page { get; set; }
        public string Sort { get; set; }
        public string SorDirection { get; set; }

        public string Filter { get; set; }

        public int PagesQuantity { get; set; }

        public IEnumerable<TDocment> Data { get; set; }

        public int TotalRows { get; set; }

        public FIlterValueClass FIlterValue{ get; set; }

    }
}
