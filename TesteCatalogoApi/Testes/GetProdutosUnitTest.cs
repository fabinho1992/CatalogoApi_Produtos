using CatalogoApi.Controllers;
using Dominio.Dtos.ProdutoDto;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesteCatalogoApi.Testes
{
    public class GetProdutosUnitTest : IClassFixture<ProdutosUnitTestController>
    {
        private readonly ProdutosController _controller;

        public GetProdutosUnitTest(ProdutosUnitTestController controller)
        {
            _controller = new ProdutosController(controller._unitToWork, controller._mapper);
        }

        [Fact]
        public async Task GetProdutoId_Return_OkResult()
        {
            //Arrange
            int prodId = 2;

            //Act
            var data = await _controller.GetById(prodId);

            //Assert
            //var resultado = Assert.IsType<OkObjectResult>(data.Result);
            //Assert.Equal(200,resultado.StatusCode);

            //Assert usando Fluentassertions
            data.Result.Should().BeOfType<OkObjectResult>()//verifica se o resultado é do tipo OkObjectResult
                .Which.StatusCode.Should().Be(200);//verifica se o resultado é um status code 200
        }

        [Fact]
        public async Task GetProdutoId_Return_BadRequest()
        {

            //Arrange
            int prodId = 0;

            //Act
            var data = await _controller.GetById(prodId);

            //Assert
            data.Result.Should().BeOfType<BadRequestObjectResult>()
                .Which.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task GetProdutoId_Return_NotFound()
        {

            //Arrange
            int prodId = 999;

            //Act
            var data = await _controller.GetById(prodId);

            //Assert
            data.Result.Should().BeOfType<NotFoundObjectResult>()
                .Which.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task GetProdutos_Return_ListaDtoOk()
        {
            //Act
            var data = await _controller.GetAll();

            //Assert
            data.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeAssignableTo<IEnumerable<ProdutoResponse>>()//verifica se o resultado além de um result ok é uma lista de produto response
                .And.NotBeNull();

        }
    }
}
