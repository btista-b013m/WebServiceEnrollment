using Microsoft.AspNetCore;
using WebServiceEnrollment;
using Serilog;
using WebServiceEnrollment.Model;
using System.Text;
using WebServiceEnrollment.Utils;

public class Program
{
    private static AppLog AppLog = new AppLog();
    public static void Main(string[] args)
    {
        //Console.WriteLine("Bienvenido a la programación con C#");
       {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo
            .File("./Logs/webServicesEnrollment.out",Serilog.Events.LogEventLevel.Debug,"{Message:lj}{NewLine}",encoding: Encoding.UTF8)
            .CreateLogger();
        
        AppLog logApp = new AppLog();
        logApp.ResponseTime = Convert.ToInt16(DateTime.Now.ToString("fff"));
        Utilerias.ImprimirLog(logApp, 0,"Iniciando servicio SOAP EnrollmentServices","Debug");
        
        CreateWebHostBuilder(args).Build().Run();
    

       }
    } 
    
    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
     WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();
}