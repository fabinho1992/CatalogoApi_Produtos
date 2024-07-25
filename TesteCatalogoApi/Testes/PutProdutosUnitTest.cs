using CatalogoApi.Controllers;
using Dominio.Dtos.ProdutoDto;
using Dominio.Modelos.Produtos;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesteCatalogoApi.Testes
{
    public class PutProdutosUnitTest : IClassFixture<ProdutosUnitTestController>
    {
        private readonly ProdutosController _controller;

        public PutProdutosUnitTest(ProdutosUnitTestController controller)
        {
            _controller = new ProdutosController(controller._unitToWork, controller._mapper);
        }

        [Fact]
        public async Task PutProdutos_Return_OkResult()
        {
            //Arrange
            var prodId = 16;

            var produto = new ProdutoPutDto
            {
                Id = prodId,
                Nome = "TesteUnit",
                Descricao = "TesteUnit",
                Preco = 2,
                Estoque = 400,
                ImagemUrl = "TesteUnit.jpg",
                CategoriaId = 2
            };

            //Act
            var data = await _controller.Update(prodId, produto);

            //Assert
            data.Should().NotBeNull();
            data.Should().BeOfType<OkObjectResult>();
            
        }
    }
}
