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

        public async Task<IEnumerable<Produto>> GetProdutosPorCategoria(int id)
        {
            return await _context.Produtos.Where(c => c.CategoriaId == id).ToListAsync();
        }
    }
}
