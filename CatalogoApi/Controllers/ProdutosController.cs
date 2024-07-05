using AutoMapper;
using Azure;
using Dominio.Dtos.ProdutoDto;
using Dominio.Interfaces;
using Dominio.Modelos;
using Infraestrutura.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
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
        public async Task<IActionResult> Create(ProdutoRequest produto)
        {
            try
            {
                if (produto is null) 
                {
                    return BadRequest();
                }

                var produtoDto = _mapper.Map<Produto>(produto);
                await _repository.ProdutoRepository.Create(produtoDto);
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

        [HttpGet("Paginados")]
        public async Task<ActionResult<IEnumerable<ProdutoResponse>>> GetPaginado([FromQuery] ProdutoPaginado produtoPaginado)
        {
            var produtos = await _repository.ProdutoRepository.GetProdutoPaginado(produtoPaginado);
            if(produtos is null)
            {
                return NotFound($"Produtos não encontrados...");
            }

            return Ok(produtos);
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

        [HttpPatch("{id}")]
        public async Task<ActionResult<ProdutoResponse>> UpdateParcial(int id, JsonPatchDocument<ProdutoUpdateRequest> produtoPatchDto)
        {
            if (produtoPatchDto is null || id <= 0)
            {
                return BadRequest();
            }

            var produto = await _repository.ProdutoRepository.Get(c => c.Id == id);
            if(produto is null)
            {
                return NotFound($"Produto com o id= {id} não encontrado...");
            }

            var produtoUpdate = _mapper.Map<ProdutoUpdateRequest>(produto);

            produtoPatchDto.ApplyTo(produtoUpdate, ModelState);
            if (!ModelState.IsValid )
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(produtoUpdate, produto);
            await _repository.ProdutoRepository.Update(produto);
            await _repository.Commit();

            return Ok();

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
