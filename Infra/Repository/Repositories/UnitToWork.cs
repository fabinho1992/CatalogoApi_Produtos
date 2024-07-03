using Dominio.Interfaces;
using Infraestrutura.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestrutura.Repository.Repositories
{
    public class UnitToWork : IUnitToWork
    {
        public IProdutoRepository? _produtoRepository;

        public ICategoriaRepository? _categoriaRepository;
        private readonly ApiDbContext _context;

        public UnitToWork(ApiDbContext context)
        {
            _context = context;
        }

        public IProdutoRepository ProdutoRepository
        {
            get { return _produtoRepository = _produtoRepository ?? new ProdutoRepository(_context); }
        }

        public ICategoriaRepository CategoriaRepository
        {
            get { return _categoriaRepository = _categoriaRepository ?? new CategoriaRepository(_context); }
        }


        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }
    }
}
