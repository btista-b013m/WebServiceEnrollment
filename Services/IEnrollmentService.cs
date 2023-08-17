// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
using System.ServiceModel;
using WebServiceEnrollment.Model;


namespace WebServiceEnrollment.Services
{
    [ServiceContract]
    public interface IEnrollmentService
    {
        [OperationContract]
        public string Test(string message);
        [OperationContract]
        EnrollmentResponse EnrollmentProcess(EnrollmentRequest request);

        [OperationContract]
        CuentaResponse EstadoCuentaProcess (CuentaRequest resquest);

        [OperationContract]
        AspiranteResponse CandidateRecordProcess(AspiranteRequest request);
    }
}