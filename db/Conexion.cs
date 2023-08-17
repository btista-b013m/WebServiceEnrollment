using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using WebServiceEnrollment.Model;
using System.Data;

namespace WebServiceEnrollment.DB
{
    public class Conexion
    {
        private static Conexion instancia;

        private IConfiguration Configuration;
        private SqlConnection connection;

        public Conexion(IConfiguration Configuration)
        {
            this.Configuration = Configuration;
            //connection = new SqlConnection("Server=localhost,1434,;Database=kalum_test;User Id=sa;Password=Inicio.2022;");
        connection = new SqlConnection(Configuration.GetConnectionString("defaultConnection"));
        }

        //como buena practica los metodos deben declararse con inicial mayúscula
        public static Conexion GetInstancia(IConfiguration Configuration)
        {
            if (instancia == null)
            {
                instancia = new Conexion(Configuration);
            }
            return instancia;
        }
       
       public AspiranteResponse ExecuteQuery(AspiranteRequest request)
       {
        AspiranteResponse response = null;
        SqlCommand cmd = new SqlCommand("sp_CandidateRecordCreate", connection);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.Add(new SqlParameter("@apellidos",request.Apellido));
        cmd.Parameters.Add(new SqlParameter("@nombres",request.Nombre));
        cmd.Parameters.Add(new SqlParameter("@direccion",request.Direccion));
        cmd.Parameters.Add(new SqlParameter("@telefono",request.Telefono));
        cmd.Parameters.Add(new SqlParameter("@email",request.Email));
        cmd.Parameters.Add(new SqlParameter("@carreraId",request.CarreraId));
        cmd.Parameters.Add(new SqlParameter("@examenId",request.ExamenId));
        cmd.Parameters.Add(new SqlParameter("@jornadaId",request.JornadaId));
        SqlDataReader reader = null;
        try
        {
            this.connection.Open();
            reader = cmd.ExecuteReader();
            
            while(reader.Read())
            {
              response = new AspiranteResponse()
              {
                Message = reader.GetValue(0).ToString(),
                NoExpediente = reader.GetValue(1).ToString()
              };
              if(reader.GetValue(0).ToString().Equals("TRANSACTION SUCCESS"))
              {
                response.StatusCode = 201;
              }
              else if(reader.GetValue(0).ToString().Equals("TRANSACTION ERROR"))
              {
                response.StatusCode = 503;
              }
              else
              {
                response.StatusCode = 500;
              }
            }
            reader.Close();
            this.connection.Close();

        }
        catch(Exception e)
        {
         response = new AspiranteResponse()
         {
            StatusCode = 500,
            Message = "Error al momento de ejecutar el proceso de creación de un aspirante en la base de datos",
            NoExpediente = "0"
         };
        }
        finally
        {
            this.connection.Close();
        }
        return response;
       }
        public EnrollmentResponse ExecuteQuery(EnrollmentRequest request)
        {
            EnrollmentResponse response = null;

            SqlCommand cmd = new SqlCommand(Configuration.GetValue<String>("Profiles:spEnrollmentProcess"), connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@noExpediente", request.NoExpediente));
            cmd.Parameters.Add(new SqlParameter("@ciclo", request.Ciclo));
            cmd.Parameters.Add(new SqlParameter("@mesInicioPago", request.MesInicioPago));
            cmd.Parameters.Add(new SqlParameter("@carreraId", request.CarreraId));
            cmd.Parameters.Add(new SqlParameter("@inscripcionCargoId", request.InscripcionCargoId));
            cmd.Parameters.Add(new SqlParameter("@carneCargoId", request.CarneCargoId));
            cmd.Parameters.Add(new SqlParameter("@cargoMensualId", request.CargoMensualId));
            cmd.Parameters.Add(new SqlParameter("@diapago", request.DiaPago));

            SqlDataReader reader = null;

            try
            {
                this.connection.Open();
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    response = new EnrollmentResponse()
                    {
                        Message = reader.GetValue(0).ToString(),
                        Carne = reader.GetValue(1).ToString()
                    };
                    if (reader.GetValue(0).ToString().Equals(Configuration.GetValue<string>("Message:MessageSuccess")))
                    {
                        response.StatusCode = Configuration.GetValue<int>("StatusCode:StatusCode1");
                    }
                    else if (reader.GetValue(0).ToString().Equals(Configuration.GetValue<string>("Message:MessageError")))
                    {
                        response.StatusCode = Configuration.GetValue<int>("StatusCode:StatusCode2");
                    }
                    else
                    {
                        response.StatusCode = Configuration.GetValue<int>("StatusCode:StatusCode3");
                    }
                }
                reader.Close();
                this.connection.Close();
            }
            catch (Exception e)
            {
                response.StatusCode = Configuration.GetValue<int>("StatusCode;StatusCode4");
                response.Message = Configuration.GetValue<string>("Message:Message");
            }
            finally
            {
                this.connection.Close();
            }

            return response;
        }

         public CuentaResponse ExecuteQuery(CuentaRequest request)
        {
            CuentaResponse response = new CuentaResponse();

            SqlCommand cmd = new SqlCommand("spEstadoCuenta", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@Carne", request.Carne));
            SqlDataReader reader = null;
            try
            {
                List<ECuentaResponse> ListaCuentas = new List<ECuentaResponse>();
                this.connection.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ECuentaResponse EstadoCuenta = new ECuentaResponse()
                    {
                        Carne = reader.GetValue(0).ToString(),
                        Nombres = reader.GetValue(1).ToString(),
                        Apellidos = reader.GetValue(2).ToString(),
                        Correlativo = reader.GetValue(3).ToString(),
                        Descripcion = reader.GetValue(4).ToString(),
                        Monto = reader.GetValue(5).ToString(),
                        Mora = reader.GetValue(6).ToString(),
                        Descuento = reader.GetValue(7).ToString()
                    };
                    ListaCuentas.Add(EstadoCuenta);

                    response.Cuentas = ListaCuentas;

                }


                reader.Close();
                this.connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.StatusCode = 503;
                response.Message = "Error al cargar la información";
            }
            finally
            {
                this.connection.Close();
            }

            return response;

        }
        
    }
}