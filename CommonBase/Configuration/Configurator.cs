using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace CommonBase.Configuration
{
    public static class Configurator
    {
        public static IConfigurationRoot LoadAppSettings()
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName ?? "Development"}.json", optional: true)
                .AddEnvironmentVariables(); 

            return builder.Build();
        }
    }
}
