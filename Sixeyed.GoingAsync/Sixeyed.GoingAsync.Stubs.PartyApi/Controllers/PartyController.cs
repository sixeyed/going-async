using Sixeyed.GoingAsync.Stubs.PartyApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sixeyed.GoingAsync.Stubs.PartyApi.Controllers
{
    public class PartyController : ApiController
    {
        private static List<PartyModel> _Models;

        public IHttpActionResult Get(string id, string scheme)
        {
            if (scheme.Equals("lei", StringComparison.InvariantCultureIgnoreCase))
            {
                var party = _Models.FirstOrDefault(x => x.LegalEntityIdentifier == id);
                if (party != null)
                {
                    return Ok(party);
                }
            }
            return NotFound();
        }

        static PartyController()
        {
            _Models = new List<PartyModel>();
            _Models.Add(new PartyModel { InternalId = "1234", LegalEntityIdentifier = "5493001RKR55V4X61F71" });
            _Models.Add(new PartyModel { InternalId = "5678", LegalEntityIdentifier = "549300O5MFEP1XJ40B46" });
            _Models.Add(new PartyModel { InternalId = "90AB", LegalEntityIdentifier = "549300OL8KL0WCQ34V31" });
            _Models.Add(new PartyModel { InternalId = "CDEF", LegalEntityIdentifier = "549300IB5Q45JGNPND58" });
        }
    }
}