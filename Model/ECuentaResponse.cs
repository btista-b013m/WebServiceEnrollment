using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace WebServiceEnrollment.Model
{
    public class ECuentaResponse
    {
        public string Carne { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Correlativo {get;set;}
        public string Descripcion {get;set;}
        public string Monto {get;set;}
        public string Mora {get;set;}
        public string Descuento {get;set;}
    }
    
}