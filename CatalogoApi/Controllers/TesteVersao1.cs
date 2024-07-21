using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CatalogoApi.Controllers
{
    [Route("api/v{version:apiVersion}/teste")]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class TesteVersao1 : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return " Testev1 - Get -> Versão 1.0 ";
        }
    }
}
