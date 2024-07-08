
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Dominio.Modelos.Categorias;

namespace Dominio.Modelos.Produtos
{
    public class Produto
    {

        public int Id { get; set; }
        public string? Nome { get; set; }
        public double Preco { get; set; }
        public string? Descricao { get; set; }
        public float Estoque { get; set; }
        public string? ImagemUrl { get; set; }
        public DateTime DataCadastro { get; set; } = DateTime.Now;
        public int CategoriaId { get; set; }
        public virtual Categoria? Categoria { get; set; }


    }
}
