using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServiceEnrollment.Model
{
    public class EnrollmentResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Carne { get; set; }
    }
}