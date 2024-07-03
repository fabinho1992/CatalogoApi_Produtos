using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Dtos.CategoriaDto
{
    public class CategoriaRequest
    {
        [Required]
        [MaxLength(50)]
        public string? Nome { get; set; }
        [Required]
        [MaxLength(300)]
        public string? ImagemUrl { get; set; }
    }
}
