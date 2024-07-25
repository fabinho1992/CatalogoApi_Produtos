using CatalogoApi.Controllers;
using Dominio.Dtos.ProdutoDto;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesteCatalogoApi.Testes
{
    public class PostProdutosUnitTest : IClassFixture<ProdutosUnitTestController>
    {
        private readonly ProdutosController _controller;

        public PostProdutosUnitTest(ProdutosUnitTestController controller)
        {
            _controller = new ProdutosController(controller._unitToWork, controller._mapper);
        }

        [Fact]
        public async Task PostCreate_return_Created()
        {
            //Arrange
            var produto = new ProdutoRequest
            {
                Nome = "Teste",
                CategoriaId = 2,
                Descricao = "Teste",
                ImagemUrl = "teste.jpg",
                Preco = 0
            };

            //Act
            var data = await _controller.Create(produto);

            //Assert
            var resultado = data.Should().BeOfType<CreatedAtRouteResult>();
            resultado.Subject.StatusCode.Should().Be(201);
                
        }

        [Fact]
        public async Task PostProdutos_Return_BadRequest()
        {
            //Arrange
            ProdutoRequest produto = null;

            //Act
            var data = await _controller.Create(produto);

            //Assert
            var resultado = data.Should().BeOfType<BadRequestResult>();
            resultado.Subject.StatusCode.Should().Be(400);
        }
    }
}
