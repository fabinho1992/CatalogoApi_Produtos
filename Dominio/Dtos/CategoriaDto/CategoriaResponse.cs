using Dominio.Dtos.ProdutoDto;
using Dominio.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Dominio.Dtos.CategoriaDto
{
    public class CategoriaResponse
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? ImagemUrl { get; set; }
        //public ICollection<ProdutoResponse>? Produtos { get; set; }

    }
}
