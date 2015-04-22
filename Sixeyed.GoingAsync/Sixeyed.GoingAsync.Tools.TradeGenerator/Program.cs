using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sixeyed.GoingAsync.Tools.TradeGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var arguments = Args.Configuration.Configure<TradeGeneratorArguments>().CreateAndBind(args);
            Generate(arguments);
        }

        private static void Generate(TradeGeneratorArguments arguments)
        {
            var outputDirectory = ConfigurationManager.AppSettings["output." + arguments.AppVersion.ToLower()];
            var source = TradeResources.Fpml_5_7;
            for (int i=0; i<arguments.Count; i++)
            {
                var fileName = Guid.NewGuid().ToString().Substring(0, 8) + ".xml";
                var outputPath = Path.Combine(outputDirectory, fileName);
                File.WriteAllText(outputPath, source);
            }
        }
    }
}
