using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FDE.Api.Controllers
{
    [Authorize]
    public class UsuarioController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, User.Identity.Name);
        }
    }
}
