using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace WebServiceEnrollment.Model
{
    public class CuentaResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public List<ECuentaResponse> Cuentas { get; set; }

    }

}