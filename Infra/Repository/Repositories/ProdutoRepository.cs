using CatalogoApi.Repository;
using Dominio.Interfaces;
using Dominio.Modelos.Categorias;
using Dominio.Modelos.Produtos;
using Infraestrutura.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestrutura.Repository.Repositories
{
    public class ProdutoRepository : RepositoryBase<Produto>, IProdutoRepository
    {
        public ProdutoRepository(ApiDbContext context) : base(context) { }

        public async Task<IEnumerable<Produto>> GetProdutosPorCategoria(string nome)
        {
            return await _context.Produtos.Where(c => c.Categoria.Nome.ToUpper() == nome.ToUpper()).Include(c => c.Categoria).ToListAsync();
        }

        public async Task<IEnumerable<Produto>> GetProdutoPaginado(ProdutoPaginado produtoPaginado)
        {
            var produtos = await _context.Produtos
                .OrderBy(p => p.Id)
                .Skip((produtoPaginado.PageNumber - 1) * produtoPaginado.PageSize)
                .Take(produtoPaginado.PageSize)
                .ToListAsync();

            return produtos;
        }

        public async Task<IEnumerable<Produto>> GetProdutoPorPreco(ProdutoPorPreco produtoPorPreco)
        {
            var produtos =  _context.Produtos.Skip((produtoPorPreco.PageNumber - 1) * produtoPorPreco.PageSize)
                .Take(produtoPorPreco.PageSize).AsQueryable();
                

            if (produtoPorPreco.Preco != null && !string.IsNullOrEmpty(produtoPorPreco.PrecoCriterio))
            {
                if (produtoPorPreco.PrecoCriterio.Equals("maior", StringComparison.OrdinalIgnoreCase))// aqui usando o stringComparison descarto se foi escrito com maiusculo ou minusculo , muito bom para não ter erro por causa disso
                {
                    produtos = produtos.Where(p => p.Preco > produtoPorPreco.Preco.Value).OrderBy(p => p.Preco);
                }
                else if (produtoPorPreco.PrecoCriterio.Equals("menor", StringComparison.OrdinalIgnoreCase))
                {
                    produtos = produtos.Where(p => p.Preco < produtoPorPreco.Preco.Value).OrderBy(p => p.Preco);
                }
                else if (produtoPorPreco.PrecoCriterio.Equals("igual", StringComparison.OrdinalIgnoreCase))
                {
                    produtos = produtos.Where(p => p.Preco == produtoPorPreco.Preco.Value).OrderBy(p => p.Preco);
                }
            }
            return produtos;
        }

    }
}
