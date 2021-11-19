﻿using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicios.api.libreria.Core.Entities
{
    [BsonCollection("Autor")]
    public class AutorEntity:Document
    {
        [BsonElement("nombre")] 
        public string Nombre { get; set; }
        [BsonElement("apellido")]
        public string Apellido { get; set; }
        [BsonElement("gradoAcademico")]
        public string GradoAcademico { get; set; }
    }
}
