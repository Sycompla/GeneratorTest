using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FBClassesODataService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IConfiguration config = null;

            config = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json", true, true)
                        .Build();

            if (config["SYSTEM"].Equals("Windows"))
            {
                CreateHostBuilder(args).Build().Run();
            } 
            else if (config["SYSTEM"].Equals("Linux"))
            {
                CreateHostBuilderKestrel(args, config["IPADDRESS"], config["PORTNUMBER"]).Build().Run();
            }
        }

        
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        
        public static IWebHostBuilder CreateHostBuilderKestrel(string[] args, string ip, string port) =>
            WebHost.CreateDefaultBuilder(args)
            .UseUrls("http://" + ip + ":" + port)
            .UseKestrel()
            .UseStartup<Startup>();
    } //Program
}