using Questao5.Domain.Entities;
using Questao5.Domain.Validation;
using Xunit;

namespace Questao5.Tests.Unidade
{
    public class ContaCorrenteTests
    {
        [Fact]
        public void Construtor_ComParametrosValidos_DeveCriarContaCorrente()
        {
            // Arrange
            int numero = 12345;
            string nome = "João da Silva";
            bool ativo = true;

            // Act
            var contaCorrente = new ContaCorrente(numero, nome, ativo);

            // Assert
            Assert.NotNull(contaCorrente.IdContaCorrente);
            Assert.Equal(numero, contaCorrente.Numero);
            Assert.Equal(nome, contaCorrente.Nome);
            Assert.True(contaCorrente.Ativo);
        }

        [Fact]
        public void Construtor_ComIdValido_DeveCriarContaCorrente()
        {
            // Arrange
            string id = "123456789";
            int numero = 12345;
            string nome = "João da Silva";
            bool ativo = true;

            // Act
            var contaCorrente = new ContaCorrente(id, numero, nome, ativo);

            // Assert
            Assert.Equal(id, contaCorrente.IdContaCorrente);
            Assert.Equal(numero, contaCorrente.Numero);
            Assert.Equal(nome, contaCorrente.Nome);
            Assert.True(contaCorrente.Ativo);
        }

        [Fact]
        public void Construtor_ComIdInvalido_DeveLancarExcecao()
        {
            // Arrange
            string id = "";
            int numero = 12345;
            string nome = "João da Silva";
            bool ativo = true;

            // Act & Assert
            var ex = Assert.Throws<DomainValidation>(() => new ContaCorrente(id, numero, nome, ativo));
            Assert.Equal("O identificador da conta corrente deve ser informado.", ex.Message);
        }

        [Fact]
        public void Numero_Nulo_DeveLancarExcecao()
        {
            // Arrange
            int? numero = null; // Número nulo
            string nome = "João da Silva";
            bool ativo = true;

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => new ContaCorrente(numero.Value, nome, ativo));
            Assert.Equal("Nullable object must have a value.", ex.Message);
        }

        [Fact]
        public void Numero_Vazio_DeveLancarExcecao()
        {
            // Arrange
            int numero = 0; // Número vazio
            string nome = "João da Silva";
            bool ativo = true;

            // Act & Assert
            var ex = Assert.Throws<DomainValidation>(() => new ContaCorrente(numero, nome, ativo));
            Assert.Equal("O número da conta corrente deve ser informado.", ex.Message);
        }

        [Fact]
        public void Nome_UltrapassaQuantidadeDesejada_DeveLancarExcecao()
        {
            // Arrange
            int numero = 12345;
            string nome = new string('A', 101); // Nome com mais de 100 caracteres
            bool ativo = true;

            // Act & Assert
            var ex = Assert.Throws<DomainValidation>(() => new ContaCorrente(numero, nome, ativo));
            Assert.Equal("O nome do titular da conta corrente deve ter até 100 caracteres.", ex.Message);
        }

        [Fact]
        public void Nome_Nulo_DeveLancarExcecao()
        {
            // Arrange
            int numero = 12345;
            string nome = null; // Nome nulo
            bool ativo = true;

            // Act & Assert
            var ex = Assert.Throws<DomainValidation>(() => new ContaCorrente(numero, nome, ativo));
            Assert.Equal("O nome do titular da conta corrente deve ser informado.", ex.Message);
        }

        [Fact]
        public void Nome_Vazio_DeveLancarExcecao()
        {
            // Arrange
            int numero = 12345;
            string nome = ""; // Nome vazio
            bool ativo = true;

            // Act & Assert
            var ex = Assert.Throws<DomainValidation>(() => new ContaCorrente(numero, nome, ativo));
            Assert.Equal("O nome do titular da conta corrente deve ser informado.", ex.Message);
        }

        [Fact]
        public void ValidateDomain_NumeroInvalido_DeveLancarExcecao()
        {
            // Arrange
            int numero = 0;
            string nome = "João da Silva";
            bool ativo = true;

            // Act & Assert
            var ex = Assert.Throws<DomainValidation>(() => new ContaCorrente(numero, nome, ativo));
            Assert.Equal("O número da conta corrente deve ser informado.", ex.Message);
        }

        [Fact]
        public void ContaCorrente_Construtor_ParametrosValidos_DeveCriarContaCorrente()
        {
            // Arrange
            int numero = 12345;
            string nome = "João da Silva";
            bool ativo = true;

            // Act
            var contaCorrente = new ContaCorrente(numero, nome, ativo);

            // Assert
            Assert.NotNull(contaCorrente.IdContaCorrente);
            Assert.Equal(numero, contaCorrente.Numero);
            Assert.Equal(nome, contaCorrente.Nome);
            Assert.True(contaCorrente.Ativo);
        }
    }
}
