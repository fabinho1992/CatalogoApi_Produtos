using Dominio.Modelos;
using Infraestrutura.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CatalogoApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public ProdutosController(ApiDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Produto produto)
        {
            try
            {
                if (produto is null) return BadRequest();
                await _context.Produtos.AddAsync(produto);
                await _context.SaveChangesAsync();

                return Ok(produto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erroo ao processar sua solicitação!");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> GetAll()
        {
            try
            {
                var produtos = await _context.Produtos.AsNoTracking().Take(10).ToListAsync();
                if (produtos is null) return NotFound("Produtos não encontrados..");
                return Ok(produtos);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erroo ao processar sua solicitação!");
            }
            
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Produto>> GetById(int id)
        {
            try
            {
                var produto = await _context.Produtos.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
                if (produto is null) return NotFound("$Produto com o id= {id} não encontrado...");
                return Ok(produto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erroo ao processar sua solicitação!");
            }
            
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, Produto request)
        {
            if(id != request.Id) return BadRequest($"Produto com o id= {id} não encontrado...");

            _context.Entry(request).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(request);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var produto = await _context.Produtos.FirstOrDefaultAsync(x => x.Id == id);
            if (produto is null) return NotFound("Produto não encontrado....");

            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();

            return Ok("Produto excluído!");
        }
    }
}
