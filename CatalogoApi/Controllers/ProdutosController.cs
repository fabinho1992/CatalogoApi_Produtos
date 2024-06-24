using Dominio.Modelos;
using Infraestrutura.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            if (produto is null) return BadRequest();
            await _context.Produtos.AddAsync(produto);
            await _context.SaveChangesAsync();

            return Ok(produto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var produtos = await _context.Produtos.ToListAsync();
            if (produtos is null) return NotFound();
            return Ok(produtos);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var produto = await _context.Produtos.FirstOrDefaultAsync(x => x.Id == id);
            if (produto is null) return NotFound();
            return Ok(produto);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, Produto request)
        {
            if(id != request.Id) return BadRequest();

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
