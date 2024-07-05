using CatalogoApi.Repository;
using Dominio.Interfaces;
using Dominio.Modelos;
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
                .OrderBy(p => p.Nome)
                .Skip((produtoPaginado.PageNumber - 1) * produtoPaginado.PageSize)
                .Take(produtoPaginado.PageSize)
                .ToListAsync();

            return produtos;
        }
    }
}
