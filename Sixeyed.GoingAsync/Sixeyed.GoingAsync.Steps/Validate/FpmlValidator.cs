using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.Schema;

namespace Sixeyed.GoingAsync.Steps.Validate
{
    public class FpmlValidator
    {
        private static readonly Dictionary<string, XmlSchemaSet> _SchemaSets = new Dictionary<string, XmlSchemaSet>();
        private static readonly object _SchemaSetLock = new object();

        private List<string> _failureMessages;

        public IEnumerable<string> Validate(string fpml)
        {
            _failureMessages = new List<string>();
            var xdoc = XDocument.Parse(fpml);

            //TODO - this would be a lookup to allow multiple schema versions:
            var version = "5-7";
            var schemaLocation = @".\schema\fpml\reporting-merged-schema-5-7\fpml-main-5-7.xsd";

            var reader = xdoc.CreateReader();
            reader.Read();

            if (!_SchemaSets.ContainsKey(version))
            {
                lock (_SchemaSetLock)
                {
                    var schemaSet = new XmlSchemaSet(reader.NameTable);
                    schemaSet.Add(@"http://www.fpml.org/FpML-5/reporting", schemaLocation);
                    schemaSet.Compile();
                    _SchemaSets[version] = schemaSet;
                }
            }

            xdoc.Validate(_SchemaSets[version], (sender, args) => _failureMessages.Add(args.Message));

            return _failureMessages;
        }
    }
}
