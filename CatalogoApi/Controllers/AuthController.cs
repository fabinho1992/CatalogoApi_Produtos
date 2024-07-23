using Dominio.Services.Token;
using Infraestrutura.Data;
using Infraestrutura.IdentityModel;
using Infraestrutura.IdentityModel.Dtos;
using Microsoft.AspNetCore.Authorization;
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
        private readonly RoleManager<IdentityRole> _roleManager;


        public AuthController(ITokenService tokenService,
                    UserManager<AppUser> userManager,
                        IConfiguration configuration,
                        RoleManager<IdentityRole> roleManager)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _configuration = configuration;
            _roleManager = roleManager;
        }

        [HttpPost("createRole")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            var roleExiste = await _roleManager.RoleExistsAsync(roleName);
            if(!roleExiste)
            {
                var role = await _roleManager.CreateAsync(new IdentityRole(roleName));

                if(role.Succeeded)
                {
                    return StatusCode(StatusCodes.Status201Created,
                        new Response { Status = "Success", Message = $"Role {roleName} created successfully!" });
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest,
                        new Response { Status = "Erro", Message = "Error creating role.." });
                }
            }

            return StatusCode(StatusCodes.Status400BadRequest, 
                  new Response { Status = "Erro", Message = "role already exists" });
        }

        [HttpPost("AddUserToRole")]
        public async Task<IActionResult> AddUserToRole(string userEmail, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);

            if(user is not null)
            {
                var addRole = await _userManager.AddToRoleAsync(user, roleName);

                if (addRole.Succeeded)
                {
                    return StatusCode(StatusCodes.Status200OK,
                        new Response { Status = "Success", Message = $"user {user.Email} added to role {roleName} successfully" });
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest,
                        new Response { Status = "Erro", Message = "error adding user to role.." });
                }
            }
            return StatusCode(StatusCodes.Status400BadRequest,
                new Response { Status = "Erro", Message = $"User {userEmail} not found.."});
        }

        /// <summary>
        /// Verifica as credenciais do usuario
        /// </summary>
        /// <param name="loginDto"> objeto loginDto</param>
        /// <returns> Status code 200 e Token de acesso do usuario</returns>
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var usuario = await _userManager.FindByNameAsync(loginDto.UserName!);

            if(usuario is not null && await _userManager.CheckPasswordAsync(usuario, loginDto.Password!))
            {
                //aqui armazeno os perfis do usuario
                var usuarioRoles = await _userManager.GetRolesAsync(usuario);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuario.UserName!),
                    new Claim(ClaimTypes.Email, usuario.Email!),
                    new Claim("id", usuario.UserName!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                foreach(var usuarioRole in usuarioRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, usuarioRole));
                }

                var token = _tokenService.GenerationAcessToken(authClaims, _configuration);//gero o token

                var tokenRefresh = _tokenService.GenerateRefreshToken();//gero o refreshToken

                _ = int.TryParse(_configuration["Jwt:RefreshTokenValidInToken"], out int refreshTokenValidInMinutes );

                usuario.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(refreshTokenValidInMinutes);// coverto o tempo do refresh para datetime e adiciono ao usuario

                usuario.Refresh = tokenRefresh;// adiciono o refresh token criado ao usuario
                
                await _userManager.UpdateAsync(usuario);//atualizo o banco de dados com o usuario

                return Ok(new
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    RefreshToken = tokenRefresh,
                    Expired = token.ValidTo
                });
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUserDto)
        {
            var usuarioExiste = await _userManager.FindByNameAsync(registerUserDto.UserName!);//consulto se o nome passado exite no banco

            if(usuarioExiste != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response { Status = "Erro", Message = "User already exists!" });// se existir passo esse erro
            }
            AppUser user = new()
            {
                UserName = registerUserDto.UserName,
                Email = registerUserDto.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var resultado = await _userManager.CreateAsync(user, registerUserDto.Password!);//crio o usuario

            if (!resultado.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new Response { Status = "Erro!", Message = "User creation failed"});
            }

            return Ok(new Response { Status = "Ok", Message = "User created successfully" });
        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
        {
            if (tokenModel == null)
            {
                return BadRequest("Invalid client request ..");
            }

            string? acessToken = tokenModel.AcessToken ?? throw new ArgumentNullException(nameof(tokenModel));
            string? refreshToken = tokenModel.RefreshToken ?? throw new ArgumentNullException( nameof(tokenModel));

            var principal = _tokenService.GetPrincipalFromExpiredToken(acessToken!, _configuration);//uso o token passado para acessar as claims

            if (principal == null)
            {
                return BadRequest("Invalid Token/ RefreshToken");
            }

            string nomeUsuario = principal.Identity.Name;// pego o nome do usuario

            var usuario = await _userManager.FindByNameAsync(nomeUsuario!);

            if(usuario is null || usuario.Refresh != refreshToken || usuario.RefreshTokenExpiryTime <= DateTime.Now)//se usuario for nulo , ou refresh token passado não for igual ao do usuario , ou se o tempo de expiração for menor que a data atual
            {
                return BadRequest("Invalid Token/ RefreshToken");
            }

            var newAcessToken = _tokenService.GenerationAcessToken(principal.Claims.ToList(), _configuration);

            var newRefreshToken = _tokenService.GenerateRefreshToken();

            usuario.Refresh = newRefreshToken;

            await _userManager.UpdateAsync(usuario);//atualizo o banco 

            return new ObjectResult(new
            {
                acessToken = new JwtSecurityTokenHandler().WriteToken(newAcessToken),
                refreshToken = newRefreshToken
            }); 
        }

        [Authorize]
        [HttpPost]
        [Route("revoke/{userName}")]
        public async Task<IActionResult> Revoke(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if(user is null)
            {
                return BadRequest("User not found!");
            }

            user.Refresh = null;

            await _userManager.UpdateAsync(user);
            
            return NoContent();
        }
    }
}
