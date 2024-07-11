using Dominio.Services.Token;
using Infraestrutura.IdentityModel;
using Infraestrutura.IdentityModel.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CatalogoApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<AppUser> _roleManager;

        public AuthController(ITokenService tokenService,
                    UserManager<AppUser> userManager,
                        IConfiguration configuration, 
                            RoleManager<AppUser> roleManager)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _configuration = configuration;
            _roleManager = roleManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var usuario = await _userManager.FindByNameAsync(loginDto.UserName!);

            if(usuario is not null && await _userManager.CheckPasswordAsync(usuario, loginDto.Password!))
            {
                //aqui armazeno os perfis do usuario
                var usuarioPerfis = _userManager.GetRolesAsync(usuario);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuario.UserName!),
                    new Claim(ClaimTypes.Email, usuario.Email!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
            }
        }
    }
}
