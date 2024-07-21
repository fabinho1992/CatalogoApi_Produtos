using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CatalogoApi.Controllers
{
    [Route("api/v{version:apiVersion}/teste")]
    [ApiController]
    [ApiVersion("2.0")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class TesteVersao2 : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return " Testev2 - Get -> Versão 2.0 ";
        }
    }
}
