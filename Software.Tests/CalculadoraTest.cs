using Xunit;

namespace Software.Tests
{
    public class CalculadoraTest
    {
        [Fact]
        public void Deve_somar_corretamente()
        {
            Software.Calculadora c = new Software.Calculadora();
            var result = c.Somar(1, 1);

            Assert.Equal(2, result);
        }
    }
}
