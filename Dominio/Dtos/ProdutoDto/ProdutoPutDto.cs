using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Dtos.ProdutoDto
{
    public class ProdutoPutDto
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public double Preco { get; set; }
        public string? Descricao { get; set; }
        public float Estoque { get; set; }
        public string? ImagemUrl { get; set; }
        public int CategoriaId { get; set; }
    }
}
