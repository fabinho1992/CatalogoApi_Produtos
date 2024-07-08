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
    public class CategoriaRepository : RepositoryBase<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(ApiDbContext context) : base(context) { }

        public async Task<IEnumerable<Categoria>> GetCategoriaPaginado(CategoriaPaginado categoriaPaginado)
        {
            var categorias = await _context.Categorias
                .OrderBy(p => p.Id)
                .Skip((categoriaPaginado.PageNumber - 1) * categoriaPaginado.PageSize)
                .Take(categoriaPaginado.PageSize)
                .ToListAsync();

            return categorias;
        }

        public async Task<IEnumerable<Categoria>> GetCategoriaPorNome(CategoriaPorNome categoriaPorNome)
        {
            var categorias = await GetAll();

            if (!string.IsNullOrEmpty(categoriaPorNome.Nome))
            {
                categorias = categorias.Where(c => c.Nome.ToUpper().Contains(categoriaPorNome.Nome.ToUpper()))
                    .Skip((categoriaPorNome.PageNumber - 1) * categoriaPorNome.PageSize)
                        .Take(categoriaPorNome.PageSize);
            }

            return categorias;
        }
    }
}
