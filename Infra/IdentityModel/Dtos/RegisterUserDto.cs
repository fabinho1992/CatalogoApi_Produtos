using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestrutura.IdentityModel.Dtos
{
    public class RegisterUserDto
    {
        [Required(ErrorMessage = "UserName obrigatório!!")]
        public string? UserName { get; set; }
        [EmailAddress]
        [Required(ErrorMessage = "Email obrigatório!!")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Password obrigatório!!")]
        public string? Password { get; set; }
    }
}
