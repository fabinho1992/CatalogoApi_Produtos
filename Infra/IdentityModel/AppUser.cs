using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestrutura.IdentityModel
{
    public class AppUser : IdentityUser
    {

        public string? Refresh { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
       
    }
}
