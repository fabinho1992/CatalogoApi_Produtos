using AutoMapper;
using Dominio.Dtos.ProdutoDto;
using Dominio.Interfaces;
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
        private readonly IUnitToWork _repository;
        private readonly IMapper _mapper;

        public ProdutosController(IUnitToWork repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Produto produto)
        {
            try
            {
                if (produto is null) 
                {
                    return BadRequest();
                }

                await _repository.ProdutoRepository.Create(produto);
                await _repository.Commit();
                return Ok(produto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao processar sua solicitação!");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> GetAll()
        {
            try
            {
                var produtos = await _repository.ProdutoRepository.GetAll();
                if (produtos is null) 
                {
                    return NotFound("Produtos não encontrados..");
                }

                return Ok(produtos);
        }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erroo ao processar sua solicitação!");
            }

}

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProdutoResponse>> GetById(int id)
        {
            try
            {
                var produto = await _repository.ProdutoRepository.Get(p => p.Id == id);
                if (produto is null) 
                {
                    return NotFound("$Produto com o id= {id} não encontrado...");
                }
                
                var produtoDto = _mapper.Map<ProdutoResponse>(produto);
                return Ok(produtoDto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erroo ao processar sua solicitação!");
            }
            
        }

        [HttpGet("PorCategoria/{nome}")]
        public async Task<IActionResult> GetPorCategoria(string nome)
        {
            var produto = await _repository.ProdutoRepository.GetProdutosPorCategoria(nome);
            if (produto is null)
            {
                return NotFound("Produto não encontrado!");
            }

            var produtoDto = _mapper.Map<IEnumerable<ProdutoResponsePorCategoria>>(produto);
            return Ok(produtoDto);
        }

        [HttpPut("{id:int:min(1)}")]//faço um filtro no parametro, que só pode receber no minímo um número 1
        public async Task<IActionResult> Update(int id, Produto request)
        {
            if (id != request.Id) 
            {
                return BadRequest($"Produto com o id= {id} não encontrado...");
            } 

            await _repository.ProdutoRepository.Update(request);
            await _repository.Commit();
            return Ok(request);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var produto = await _repository.ProdutoRepository.Get(p => p.Id == id);
            if (produto is null) 
            {
                return NotFound("Produto não encontrado....");
            }

            await _repository.ProdutoRepository.Delete(produto);
            await _repository.Commit();
            return Ok("Produto excluído!");
        }
    }
}
