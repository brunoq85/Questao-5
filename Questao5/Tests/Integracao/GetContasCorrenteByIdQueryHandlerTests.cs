using Microsoft.Data.Sqlite;
using Moq;
using Questao5.Application.ContaCorrente.Queries;
using Questao5.Application.ContaCorrente.Queries.Handler;
using Questao5.Domain.Interfaces;
using Questao5.Infrastructure.Database.QueryStore.Requests.ContaCorrente;
using Questao5.Infrastructure.Database.QueryStore.Responses.ContaCorrente;
using System.Data;
using Xunit;

namespace Questao5.Tests.Integracao
{
    public class GetContasCorrenteByIdQueryHandlerTests : IDisposable
    {
        private readonly IDbConnection _connection;
        private readonly Mock<IContaCorrenteRepository> _repositoryMock;
        private readonly GetContasCorrenteByIdQueryHandler _handler;

        public GetContasCorrenteByIdQueryHandlerTests()
        {
            _connection = new SqliteConnection("Data Source=:memory:");
            _connection.Open();

            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = @"
                CREATE TABLE contacorrente (
                    idcontacorrente TEXT(37) PRIMARY KEY, 
                    numero INTEGER(10) NOT NULL UNIQUE, 
                    nome TEXT(100) NOT NULL, 
                    ativo INTEGER(1) NOT NULL default 0, 
	                CHECK (ativo in (0,1)))";
                cmd.ExecuteNonQuery();
            }

            _repositoryMock = new Mock<IContaCorrenteRepository>();
            _handler = new GetContasCorrenteByIdQueryHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnContaCorrente_WhenContaCorrenteExists()
        {
            // Arrange
            var request = new GetContasCorrenteByIdQuery("1");
            var response = new GetContaCorrenteByIdResponse
            {
                IdContaCorrente = "1",
                Numero = 123456,
                Nome = "Teste",
                Ativo = true
            };

            _repositoryMock.Setup(repo => repo.GetContaCorrenteById(It.IsAny<GetContaCorrenteByIdRequest>()))
                           .ReturnsAsync(response);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(request.IdContaCorrente, result.IdContaCorrente);
            Assert.Equal(response.Numero, result.Numero);
            Assert.Equal(response.Nome, result.Nome);
            Assert.True(result.Ativo);
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenContaCorrenteDoesNotExist()
        {
            // Arrange
            var request = new GetContasCorrenteByIdQuery("2");

            _repositoryMock.Setup(repo => repo.GetContaCorrenteById(It.IsAny<GetContaCorrenteByIdRequest>()))
                           .ReturnsAsync((GetContaCorrenteByIdResponse)null);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}
