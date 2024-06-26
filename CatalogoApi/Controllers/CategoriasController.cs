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
                var categorias = await _context.Categorias.AsNoTracking().Take(10).Include(c => c.Produtos).ToListAsync();
                if(categorias is null)
                {
                    return NotFound("Categorias não encontradas...");
                }
                return Ok(categorias);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erroo ao processar sua solicitação!");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateAsync(int id, Categoria request)
        {
            if(id != request.Id)
            {
                return BadRequest($"Categoria com o id= {id} não encontrado...");
            }
            _context.Entry(request).State = EntityState.Modified; 
            await _context.SaveChangesAsync();
            return Ok(request);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var categoria = await _context.Categorias.FirstOrDefaultAsync(x => x.Id == id);
            if (categoria is null) return NotFound("Categoria não encontrado....");

            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();

            return Ok("Categoria excluída!");
        }

    }
}
