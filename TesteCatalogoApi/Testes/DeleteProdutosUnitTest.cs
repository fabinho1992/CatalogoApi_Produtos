using CatalogoApi.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesteCatalogoApi.Testes
{
    public class DeleteProdutosUnitTest : IClassFixture<ProdutosUnitTestController>
    {
        private readonly ProdutosController _controller;

        public DeleteProdutosUnitTest(ProdutosUnitTestController controller)
        {
            _controller = new ProdutosController(controller._unitToWork, controller._mapper);
        }

        [Fact]
        public async Task DeleteProdutos_Return_OkResult()
        {
            //Arrange
            var prodId = 4;

            //Act
            var data = await _controller.Delete(prodId);

            //Assert
            data.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(200);
            
        }

        [Fact]
        public async Task DeleteProdutos_Return_NotFoundResult()
        {
            //Arrange
            var prodId = 4;

            //Act
            var data = await _controller.Delete(prodId);

            //Assert
            data.Should().BeOfType<NotFoundObjectResult>().Which.StatusCode.Should().Be(404);

        }
    }
}
