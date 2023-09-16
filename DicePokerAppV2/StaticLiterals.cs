using CommonBase.Configuration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DicePokerAppV2
{
    internal static class StaticLiterals
    {
        static StaticLiterals()
        {
            var config = Configurator.LoadAppSettings();


            if(config != null)
                DefaultNames = config.GetSection("DefaultNames").Get<string[]>();

        }

        internal static string[]? DefaultNames;
    }
}
