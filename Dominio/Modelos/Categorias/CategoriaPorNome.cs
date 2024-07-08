using Dominio.Modelos.Abstrato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Modelos.Categorias
{
    public class CategoriaPorNome : ParametrosPaginacao
    {
        public string? Nome { get; set; }
    }
}
