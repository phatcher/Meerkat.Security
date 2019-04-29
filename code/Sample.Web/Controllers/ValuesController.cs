using System.Collections.Generic;
using System.Net;
using System.Web.Http;

namespace Sample.Web.Controllers
{
    [RoutePrefix("api/values")]
    public class ValuesController : ApiController
    {        
        [HttpGet]
        [Route("secure")]
        public IHttpActionResult Secure()
        {
            // NB Not sure why User is null - thought default principal would be assigned.
            if (User == null || User.Identity.IsAuthenticated == false)
            {
                return Unauthorized();
            }
            return Ok(new [] { "A", "B" });
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("insecure")]
        public IHttpActionResult Insecure()
        {
            return Ok(new[] { "C", "D" });
        }

        [HttpPost]
        [Route("update")]
        public int Update([FromBody] string value)
        {
            if (User == null || User.Identity.IsAuthenticated == false)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

            return 1;
        }
    }
}