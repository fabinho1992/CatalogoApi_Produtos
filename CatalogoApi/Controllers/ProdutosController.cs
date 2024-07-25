using AutoMapper;
using Dominio.Dtos.ProdutoDto;
using Dominio.Interfaces;
using Dominio.Modelos.Produtos;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CatalogoApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    //[ApiConventionType(typeof(DefaultApiConventions))]// padronização dos tipos de respostas que tenho no swagger
    //[ApiExplorerSettings(IgnoreApi = true)]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitToWork _repository;
        private readonly IMapper _mapper;

        public ProdutosController(IUnitToWork repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> Create(ProdutoRequest produto)
        {
            try
            {
                if (produto is null)
                {
                    return BadRequest();
                }

                var novoProduto = _mapper.Map<Produto>(produto);
                await _repository.ProdutoRepository.Create(novoProduto);
                await _repository.Commit();
                return new CreatedAtRouteResult("GetById", new { id = novoProduto.Id}, novoProduto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao processar sua solicitação!");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoResponse>>> GetAll()
        {
            try
            {
                var produtos = await _repository.ProdutoRepository.GetAll();
                if (produtos is null)
                {
                    return NotFound("Produtos não encontrados..");
                }
                var produtosDto = _mapper.Map <IEnumerable<ProdutoResponse>>(produtos);
                return Ok(produtosDto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erroo ao processar sua solicitação!");
            }

        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProdutoResponse>> GetById(int? id)
        {
            if(id is null || id <= 0)
            {
                return BadRequest("Coloque um Id maior que 0");
            }

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
            if (produtos is null)
            {
                return NotFound($"Produtos não encontrados...");
            }

            var produtosDto = _mapper.Map<IEnumerable<ProdutoResponse>>(produtos);
            return Ok(produtosDto);
        }

        [HttpGet("PorPreco")]
        public async Task<ActionResult<IEnumerable<ProdutoResponse>>> GetPorPreco([FromQuery] ProdutoPorPreco produtoPorPreco)
        {
            var produtos = await _repository.ProdutoRepository.GetProdutoPorPreco(produtoPorPreco);
            if (produtos is null)
            {
                return NotFound($"Produtos não encontrados...");
            }

            var produtosDto = _mapper.Map<IEnumerable<ProdutoResponse>>(produtos);
            return Ok(produtosDto);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("{id:int:min(1)}")]//faço um filtro no parametro, que só pode receber no minímo um número 1
        public async Task<ActionResult> Update(int id, ProdutoPutDto request)
        {
            if (id != request.Id) 
            {
                return BadRequest($"Produto com o id= {id} não encontrado...");
            }

            var produtoDto = _mapper.Map<Produto>(request);
            await _repository.ProdutoRepository.Update(produtoDto);
            await _repository.Commit();
            return Ok(produtoDto);
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
