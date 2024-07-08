using Dominio.Interfaces.Generic;
using Dominio.Modelos.Categorias;
using Dominio.Modelos.Produtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Interfaces
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<IEnumerable<Produto>> GetProdutoPaginado(ProdutoPaginado produtoPaginado);
        Task<IEnumerable<Produto>> GetProdutosPorCategoria(string nome);
        Task<IEnumerable<Produto>> GetProdutoPorPreco(ProdutoPorPreco produtoPorPreco);
    }
}
