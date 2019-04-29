using System.Collections.Generic;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Sample.AspNetCore.Controllers
{
    [Route("[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new [] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return User.Identity.Name + "/" + User.Identity.IsAuthenticated;
        }

        // GET api/values/5
        [Authorize]
        [HttpGet("{id}/authenticated")]
        public string Authenticated(int id)
        {
            return User.Identity.Name + "/" + User.Identity.IsAuthenticated;
        }
    }
}
