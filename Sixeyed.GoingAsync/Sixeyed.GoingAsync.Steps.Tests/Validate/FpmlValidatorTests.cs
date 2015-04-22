using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sixeyed.GoingAsync.Steps.Validate;
using System.IO;
using System.Linq;

namespace Sixeyed.GoingAsync.Steps.Tests.Validate
{
    [TestClass]
    [DeploymentItem("Resources", "Resources")]
    [DeploymentItem("schema", "schema")]
    public class FpmlValidatorTests
    {
        [TestMethod]
        public void ValidFmplPassesValidation()
        {
            var fpml = File.ReadAllText(@"Resources\Fpml-Valid_5-7.xml");
            var validator = new FpmlValidator();
            var failures = validator.Validate(fpml);
            Assert.IsNotNull(failures);
            Assert.IsFalse(failures.Any());
        }

        [TestMethod]
        public void InvalidFmplFailsValidation()
        {
            var fpml = File.ReadAllText(@"Resources\Fpml-Invalid_5-7.xml");
            var validator = new FpmlValidator();
            var failures = validator.Validate(fpml);
            Assert.IsNotNull(failures);
            Assert.AreEqual(2, failures.Count());
            Assert.AreEqual(1, failures.Count(x => x == @"The 'http://www.fpml.org/FpML-5/reporting:isCorrection' element is invalid - The value 'maybe' is invalid according to its datatype 'http://www.w3.org/2001/XMLSchema:boolean' - The string 'maybe' is not a valid Boolean value."));
            Assert.AreEqual(1, failures.Count(x => x == "Reference to undeclared ID is 'party1'."));
        }
    }
}
