using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sixeyed.GoingAsync.Core
{
    public static class Config
    {
        public static string Get(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
