using CommonBase.Configuration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DicePokerAppV2
{
    internal static class StaticLiterals
    {
        internal static string[]? DefaultNames;

        public static void LoadSettings()
        {
            var config = Configurator.LoadAppSettings();

            if (config != null)
                DefaultNames = config.GetSection("DefaultNames").Get<string[]>();
        }
    }
}
