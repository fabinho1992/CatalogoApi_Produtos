using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Services.Token
{
    public class TokenService : ITokenService
    {
        public string GenerateRefreshToken() // para obter um novo token, o usuario não precisa colocar suas credencias novamente
        {
            var secureRandomBytes = new byte[128];

            using var randomNumberGenerator = RandomNumberGenerator.Create(); // criar numeros aleatórios

            randomNumberGenerator.GetBytes(secureRandomBytes); // gero os numeros e armazeno em secureRandomBytes

            var refreshToken = Convert.ToBase64String(secureRandomBytes);// converto para melhor amarzenamento
            return refreshToken;
        }

        public JwtSecurityToken GenerationAcessToken(IEnumerable<Claim> claims, IConfiguration _config) // método que cria um token 
        {
            var keyString = _config.GetSection("Jwt").GetValue<string>("SecretKey") ?? throw new InvalidOperationException("Invalid secret Key!!");

            var key = Encoding.UTF8.GetBytes(keyString);

            var assinaturaCredencialChave = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature); // essas 3 variaveis são usadas para armazenar a chave secreta pronta para uso no token

            var descricaoToken = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_config.GetSection("Jwt")
                                                        .GetValue<double>("TokenValidityInMinutes")),
                Issuer = _config.GetSection("Jwt").GetValue<string>("ValidIssuer"),
                Audience = _config.GetSection("Jwt").GetValue<string>("ValidAudience"),
                SigningCredentials = assinaturaCredencialChave
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateJwtSecurityToken(descricaoToken); // aqui crio o token
            return token;
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token, IConfiguration _config)
        {
            var secretKey = _config["Jwt:SecretKey"] ?? throw new InvalidOperationException("Invalid Key!");

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                        !jwtSecurityToken.Header.Alg.Equals
                        (SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid Token!");
            }
            return principal;

        }
    }

    
}
