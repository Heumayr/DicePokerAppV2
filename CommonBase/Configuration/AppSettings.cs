using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonBase.Configuration
{
    public static class AppSettings
    {
        private static IConfiguration? configuration;

        public static IConfiguration Configuration
        {
            get => configuration ??= Configurator.LoadAppSettings();
            set => configuration = value;
        }

        public static string? Get(string key)
        {
            var result = default(string);

            if (Configuration != null)
            {
                result = Configuration[key];
            }
            return result;
        }

        public static IConfigurationSection? GetSection(string key)
        {
            var result = default(IConfigurationSection);

            if (Configuration != null)
            {
                result = Configuration.GetSection(key);
            }
            return result;
        }
    }
}
