using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Modelos
{
    public class Produto
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public double Preco { get; set; }
        public string? Descricao { get; set;}
        public float Estoque { get; set; }
        public string? ImagemUrl { get; set; }
        public DateOnly DataCadastro { get; set; }
        public int CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }

    }
}
