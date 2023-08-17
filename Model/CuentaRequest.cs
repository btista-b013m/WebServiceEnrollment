using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace WebServiceEnrollment.Model
{
    public class CuentaRequest
    {
        [DataMember]
        public string Carne { get; set; }
    }
}