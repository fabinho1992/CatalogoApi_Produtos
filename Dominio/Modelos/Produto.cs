using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
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
        [JsonIgnore]
        public DateTime DataCadastro { get; set; } = DateTime.Now;
        public int CategoriaId { get; set; }
        [JsonIgnore]
        public Categoria? Categoria { get; set; }

    }
}
