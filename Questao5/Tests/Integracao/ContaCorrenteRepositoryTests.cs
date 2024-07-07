using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Domain.Interfaces;
using Questao5.Infrastructure.Database.CommandStore.Requests.ContaCorrente;
using Questao5.Infrastructure.Database.QueryStore.Requests.ContaCorrente;
using Questao5.Infrastructure.Repository;
using System.Data;
using Xunit;

namespace Questao5.Tests.Integracao
{
    public class ContaCorrenteRepositoryTests
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ContaCorrenteRepository _repository;
        private readonly IDbConnection _connection;

        public ContaCorrenteRepositoryTests()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();
            _unitOfWork = new UnitOfWork(_connection);
            _repository = new ContaCorrenteRepository(_unitOfWork, _connection);

            // Setup the schema
            _connection.Execute(@"CREATE TABLE contacorrente (
    idcontacorrente TEXT(37) PRIMARY KEY, 
    numero INTEGER(10) NOT NULL UNIQUE, 
    nome TEXT(100) NOT NULL, 
    ativo INTEGER(1) NOT NULL default 0, 
	CHECK (ativo in (0,1)))");
        }

        [Fact]
        public async Task GetAllContasCorrente_ReturnsAllRecords()
        {
            // Arrange
            var command = @"INSERT INTO contacorrente 
(idcontacorrente, numero, nome, ativo) 
VALUES ('1', 123, 'Conta 1', 0), 
        ('2', 456, 'Conta 2', 0)";
            await _connection.ExecuteAsync(command, transaction: _unitOfWork.Transaction);

            // Act
            var result = await _repository.GetAllContasCorrente();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, c => c.IdContaCorrente == "1" && c.Nome == "Conta 1");
            Assert.Contains(result, c => c.IdContaCorrente == "2" && c.Nome == "Conta 2");
        }

        [Fact]
        public async Task GetContaCorrenteById_ReturnsCorrectContaCorrente()
        {
            // Arrange
            var command = "INSERT INTO contacorrente (idcontacorrente, numero, nome, ativo) VALUES ('1', 123, 'Conta 1', 1)";
            await _connection.ExecuteAsync(command, transaction: _unitOfWork.Transaction);

            var request = new GetContaCorrenteByIdRequest { IdContaCorrente = "1" };

            // Act
            var result = await _repository.GetContaCorrenteById(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("1", result.IdContaCorrente);
            Assert.Equal("Conta 1", result.Nome);
        }

        [Fact]
        public async Task AddContaCorrente_AddsNewContaCorrente()
        {
            // Arrange
            var request = new AddContaCorrenteRequest
            {
                IdContaCorrente = "2",
                Numero = 789,
                Nome = "Conta 3",
                Ativo = true
            };

            // Act
            var result = await _repository.AddContaCorrente(request);

            // Assert
            Assert.Equal(request.IdContaCorrente, result.IdContaCorrente);
            Assert.Equal(request.Numero, result.Numero);
            Assert.Equal(request.Nome, result.Nome);
            Assert.Equal(request.Ativo, result.Ativo);

            var contaCorrente = await _connection.QuerySingleOrDefaultAsync<ContaCorrente>(
                "SELECT * FROM contacorrente WHERE idcontacorrente = @IdContaCorrente",
                new { IdContaCorrente = request.IdContaCorrente }
            );

            Assert.NotNull(contaCorrente);
        }

        [Fact]
        public async Task DeleteContaCorrente_DeletesContaCorrente()
        {
            // Arrange
            var command = "INSERT INTO contacorrente (idcontacorrente, numero, nome, ativo) VALUES (1, 123, 'Conta 1', 1)";
            await _connection.ExecuteAsync(command, transaction: _unitOfWork.Transaction);

            var request = new DeleteContaCorrenteRequest { IdContaCorrente = "1" };

            // Act
            var result = await _repository.DeleteContaCorrente(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("1", result.IdContaCorrente);

            var contaCorrente = await _connection.QuerySingleOrDefaultAsync<ContaCorrente>(
                "SELECT * FROM contacorrente WHERE idcontacorrente = @IdContaCorrente",
                new { IdContaCorrente = request.IdContaCorrente }
            );

            Assert.Null(contaCorrente);
        }

        [Fact]
        public async Task UpdateContaCorrente_UpdatesContaCorrente()
        {
            // Arrange
            var command = "INSERT INTO contacorrente (idcontacorrente, numero, nome, ativo) VALUES (1, 123, 'Conta 1', 1)";
            await _connection.ExecuteAsync(command, transaction: _unitOfWork.Transaction);

            var request = new UpdateContaCorrenteRequest
            {
                IdContaCorrente = "1",
                Numero = 456,
                Nome = "Conta Atualizada",
                Ativo = false
            };

            // Act
            var result = await _repository.UpdateContaCorrente(request);

            // Assert
            Assert.Equal(request.IdContaCorrente, result.IdContaCorrente);
            Assert.Equal(request.Numero, result.Numero);
            Assert.Equal(request.Nome, result.Nome);
            Assert.Equal(request.Ativo, result.Ativo);

            var contaCorrente = await _connection.QuerySingleOrDefaultAsync<ContaCorrente>(
                "SELECT * FROM contacorrente WHERE idcontacorrente = @IdContaCorrente",
                new { IdContaCorrente = request.IdContaCorrente }
            );

            Assert.Equal(request.Numero, contaCorrente.Numero);
            Assert.Equal(request.Nome, contaCorrente.Nome);
            Assert.Equal(request.Ativo, contaCorrente.Ativo);
        }
    }
}
