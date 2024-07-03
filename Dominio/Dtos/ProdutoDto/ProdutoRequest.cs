using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Dtos.ProdutoDto
{
    public class ProdutoRequest
    {
        [Required]
        [MaxLength(50)]
        public string? Nome { get; set; }
        [Required]
        public double Preco { get; set; }
        public string? Descricao { get; set; }
        [MaxLength(300)]
        public string? ImagemUrl { get; set; }
        [Required]
        public int CategoriaId { get; set; }
    }
}
