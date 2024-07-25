namespace TesteCatalogoApi.Testes
{
    public class UnitTest1
    {
        [Fact]
        public void TesteSeVerdadeiro()
        {
            //Arrange
            int valorEsperado = 9;
            int valor1 = 5;
            int valor2 = 4;

            //Act
            var resultado = valor1 + valor2;

            //Assert
            Assert.False(resultado > valorEsperado);
        }
    }
}