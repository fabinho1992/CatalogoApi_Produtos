using Dominio.Modelos;
using Infraestrutura.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogoApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public CategoriasController(ApiDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult> Create(Categoria categoria)
        {
            try
            {
                if (categoria is null)
                {
                    return BadRequest();
                }

                await _context.Categorias.AddAsync(categoria);
                await _context.SaveChangesAsync();
                return Ok(categoria);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erroo ao processar sua solicitação!");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetAll()
        {
            try
            {
                var categorias = await _context.Categorias.AsNoTracking().Include(c => c.Produtos).ToListAsync();
                if(categorias is null)
                {
                    return BadRequest("Categorias não encontradas...");
                }
                return Ok(categorias);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erroo ao processar sua solicitação!");
            }
        }
    }
}
