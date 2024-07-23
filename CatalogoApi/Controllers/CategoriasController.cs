using AutoMapper;
using Dominio.Dtos.CategoriaDto;
using Dominio.Dtos.ProdutoDto;
using Dominio.Interfaces;
using Dominio.Interfaces.Generic;
using Dominio.Modelos.Categorias;
using Dominio.Modelos.Produtos;
using Infraestrutura.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogoApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitToWork _repository;
        private readonly IMapper _mapper;

        public CategoriasController(IUnitToWork repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        //[Authorize(Policy = "UserOnly")]
        public async Task<ActionResult> Create(CategoriaRequest categoria)
        {
            try
            {
                if (categoria is null)
                {
                    return BadRequest();
                }

                var categoriaDto = _mapper.Map<Categoria>(categoria);
                await _repository.CategoriaRepository.Create(categoriaDto);
                await _repository.Commit();
                return Created();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erroo ao processar sua solicitação!");
            }
        }

        /// <summary>
        /// Obtem uma lista de objetos Categorias
        /// </summary>
        /// <returns> Uma lista de objetos Categorias </returns>
        //[Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaResponse>>> GetAll()
        {
            try
            {
                var categorias = await _repository.CategoriaRepository.GetAll();
                if (categorias is null)
                {
                    return NotFound("Categorias não encontradas...");
                }
                var categoriasDto = _mapper.Map<IEnumerable<CategoriaResponse>>(categorias);
                return Ok(categoriasDto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erroo ao processar sua solicitação!");
            }
        }

        //[Authorize]
        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById(int id)
        {
            var categoria = await _repository.CategoriaRepository.Get(c => c.Id == id);
            if (categoria is null)
            {
                return BadRequest($"Categoria com o id= {id} não encontrado...");
            }

            var categoriaDto = _mapper.Map<CategoriaResponse>(categoria);
            return Ok(categoriaDto);
        }

        [HttpGet("Paginados")]
        public async Task<ActionResult<IEnumerable<CategoriaResponse>>> GetPaginado([FromQuery] CategoriaPaginado categoriaPaginado)
        {
            var categorias = await _repository.CategoriaRepository.GetCategoriaPaginado(categoriaPaginado);
            if (categorias is null)
            {
                return NotFound($"Produtos não encontrados...");
            }

            var categoriasDto = _mapper.Map<IEnumerable<CategoriaResponse>>(categorias);
            return Ok(categoriasDto);
        }

        [HttpGet("CategoriaPorNome")]
        public async Task<ActionResult<IEnumerable<CategoriaResponse>>> GetPorNome([FromQuery] CategoriaPorNome categoriaPorNome)
        {
            var categorias = await _repository.CategoriaRepository.GetCategoriaPorNome(categoriaPorNome);
            if (categorias is null)
            {
                return NotFound($"Produtos não encontrados...");
            }

            var categoriasDto = _mapper.Map<IEnumerable<CategoriaResponse>>(categorias);
            return Ok(categoriasDto);
        }

        [HttpPut("{id:int}")]
        [ApiConventionMethod(typeof(DefaultApiConventions),nameof(DefaultApiConventions.Put))]
        public async Task<ActionResult> UpdateAsync(int id, Categoria request)
        {
            if(id != request.Id)
            {
                return BadRequest($"Categoria com o id= {id} não encontrado...");
            }
            
            await _repository.CategoriaRepository.Update(request);
            await _repository.Commit();
            return Ok(request);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var categoria = await _repository.CategoriaRepository.Get(c => c.Id == id);
            if (categoria is null) 
            {
                return NotFound("Categoria não encontrado....");
            } 

            await _repository.CategoriaRepository.Delete(categoria);    
            await _repository.Commit();
            return Ok("Categoria excluída!");
        }

    }
}
