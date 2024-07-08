using Dominio.Interfaces.Generic;
using Dominio.Modelos.Categorias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Interfaces
{
    public interface ICategoriaRepository : IRepository<Categoria> 
    {
        Task<IEnumerable<Categoria>> GetCategoriaPaginado(CategoriaPaginado categoriaPaginado);
        Task<IEnumerable<Categoria>> GetCategoriaPorNome(CategoriaPorNome categoriaPorNome);
    }
}
