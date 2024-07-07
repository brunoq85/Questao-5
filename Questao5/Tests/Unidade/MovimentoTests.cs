using Questao5.Domain.Entities;
using Questao5.Domain.Validation;
using Xunit;

namespace Questao5.Tests.Unidade
{
    public class MovimentoTests
    {
        [Fact]
        public void IdMovimento_UltrapassaQuantidadeDesejada_DeveLancarExcecao()
        {
            // Arrange
            string idMovimento = new string('A', 38); // IdMovimento com mais de 37 caracteres
            string idContaCorrente = "12345";
            DateTime dataMovimento = DateTime.Now;
            char tipoMovimento = 'C';
            decimal valor = 100m;

            // Act & Assert
            var ex = Assert.Throws<DomainValidation>(() => new Movimento(idMovimento, idContaCorrente, dataMovimento, tipoMovimento, valor));
            Assert.Equal("O identificador do movimento deve ter até 37 caracteres", ex.Message);
        }

        [Fact]
        public void IdMovimento_Nulo_DeveLancarExcecao()
        {
            // Arrange
            string idMovimento = null; // IdMovimento nulo
            string idContaCorrente = "12345";
            DateTime dataMovimento = DateTime.Now;
            char tipoMovimento = 'C';
            decimal valor = 100m;

            // Act & Assert
            var ex = Assert.Throws<DomainValidation>(() => new Movimento(idMovimento, idContaCorrente, dataMovimento, tipoMovimento, valor));
            Assert.Equal("O identificador do movimento deve ser informado.", ex.Message);
        }

        [Fact]
        public void IdMovimento_Vazio_DeveLancarExcecao()
        {
            // Arrange
            string idMovimento = ""; // IdMovimento vazio
            string idContaCorrente = "12345";
            DateTime dataMovimento = DateTime.Now;
            char tipoMovimento = 'C';
            decimal valor = 100m;

            // Act & Assert
            var ex = Assert.Throws<DomainValidation>(() => new Movimento(idMovimento, idContaCorrente, dataMovimento, tipoMovimento, valor));
            Assert.Equal("O identificador do movimento deve ser informado.", ex.Message);
        }

        [Fact]
        public void IdContaCorrente_UltrapassaQuantidadeDesejada_DeveLancarExcecao()
        {
            // Arrange
            string idContaCorrente = new string('A', 38); // IdContaCorrente com mais de 37 caracteres
            char tipoMovimento = 'C';
            decimal valor = 100m;

            // Act & Assert
            var ex = Assert.Throws<DomainValidation>(() => new Movimento(idContaCorrente, tipoMovimento, valor));
            Assert.Equal("O identificador da conta corrente deve ter até 37 caracteres", ex.Message);
        }

        [Fact]
        public void IdContaCorrente_Nulo_DeveLancarExcecao()
        {
            // Arrange
            string idContaCorrente = null; // IdContaCorrente nulo
            char tipoMovimento = 'C';
            decimal valor = 100m;

            // Act & Assert
            var ex = Assert.Throws<DomainValidation>(() => new Movimento(idContaCorrente, tipoMovimento, valor));
            Assert.Equal("O identificador da conta corrente deve ser informado.", ex.Message);
        }

        [Fact]
        public void IdContaCorrente_Vazio_DeveLancarExcecao()
        {
            // Arrange
            string idContaCorrente = ""; // IdContaCorrente vazio
            char tipoMovimento = 'C';
            decimal valor = 100m;

            // Act & Assert
            var ex = Assert.Throws<DomainValidation>(() => new Movimento(idContaCorrente, tipoMovimento, valor));
            Assert.Equal("O identificador da conta corrente deve ser informado.", ex.Message);
        }

        [Fact]
        public void DataMovimento_MenorQueDataAtual_DeveLancarExcecao()
        {
            // Arrange
            string idMovimento = Guid.NewGuid().ToString();
            string idContaCorrente = "12345";
            DateTime dataMovimento = DateTime.Now.AddDays(-1); // Data menor que a atual
            char tipoMovimento = 'C';
            decimal valor = 100m;

            // Act & Assert
            var ex = Assert.Throws<DomainValidation>(() => new Movimento(idMovimento, idContaCorrente, dataMovimento, tipoMovimento, valor));
            Assert.Equal("Data inválida", ex.Message);
        }

        [Fact]
        public void Valor_DeveSerPositivo()
        {
            // Arrange
            string idContaCorrente = "12345";
            char tipoMovimento = 'C';
            decimal valor = -100m; // Valor negativo

            // Act & Assert
            var ex = Assert.Throws<DomainValidation>(() => new Movimento(idContaCorrente, tipoMovimento, valor));
            Assert.Equal("O valor do movimento deve ser positivo.", ex.Message);
        }

        [Fact]
        public void TipoMovimento_DeveSerC_ou_D()
        {
            // Arrange
            string idContaCorrente = "12345";
            char tipoMovimento = 'X'; // Tipo inválido
            decimal valor = 100m;

            // Act & Assert
            var ex = Assert.Throws<DomainValidation>(() => new Movimento(idContaCorrente, tipoMovimento, valor));
            Assert.Equal("Tipo de movimento inválido. Use 'C' para crédito ou 'D' para débito.", ex.Message);
        }
    }
}
