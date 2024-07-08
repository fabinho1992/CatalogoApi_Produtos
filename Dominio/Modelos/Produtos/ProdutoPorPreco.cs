using Dominio.Modelos.Abstrato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Modelos.Produtos
{
    public class ProdutoPorPreco : ParametrosPaginacao
    {
        public double? Preco { get; set; }
        public string? PrecoCriterio { get; set; }//maior / menor / igual 
    }
}
