using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RivitaBackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //logger will allow to define some default expected behaviours for our logger
            //write to File, set path. those logs will be saved in your defined path.
            //then we define outputTemplate. how output will look like. there will be time
            //rollingInterval means that with each log file you will have day associated with it like log-2020-09-25.txt
            //if something happened that day you can easily find it. other way is to have one big file with years of logs. its a mess
            //THEN i have restrictedToMinimumLevel. i want to log only minimum of information. So THEN we can create Logger
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(path: "c:\\rivitabackend\\logs\\log-.txt",
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception]",
                rollingInterval: RollingInterval.Day,
                restrictedToMinimumLevel: LogEventLevel.Information
                ).CreateLogger();
            try
            {
                //when we start our application. write to file that application is working
                Log.Information("Application Is Starting");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                //if exception happened it will write it to our file where Logs are
                Log.Fatal(ex, "Application Failed to start");
            }
            finally
            {
                //after operation is done flush and close Log object
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
             Host.CreateDefaultBuilder(args)
                 .UseSerilog()
                 .ConfigureWebHostDefaults(webBuilder =>
                 {
                     webBuilder.UseStartup<Startup>();
                 });
    }
}
