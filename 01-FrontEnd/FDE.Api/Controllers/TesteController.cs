using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FDE.Api.Controllers
{
    [AllowAnonymous]
    public class TesteController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, "Tudo em ordem");
        }
    }
}
