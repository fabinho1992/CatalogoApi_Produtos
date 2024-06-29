using CatalogoApi.Repository;
using Dominio.Interfaces;
using Dominio.Modelos;
using Infraestrutura.Data;
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
    }
}
