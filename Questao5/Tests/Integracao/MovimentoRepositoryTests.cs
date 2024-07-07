using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database.CommandStore.Requests.Movimento;
using Questao5.Infrastructure.Repository;
using System.Data;
using Xunit;

namespace Questao5.Tests.Integracao
{
    public class MovimentoRepositoryTests
    {
        private readonly IDbConnection _connection;
        private readonly UnitOfWork _unitOfWork;
        private readonly MovimentoRepository _repository;

        public MovimentoRepositoryTests()
        {
            // Configure a conexão com o banco de dados de teste
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();
            _unitOfWork = new UnitOfWork(_connection);

            // Cria a tabela para o teste
            _connection.Execute(@"
            CREATE TABLE movimento (
                idmovimento TEXT PRIMARY KEY,
                idcontacorrente TEXT NOT NULL,
                datamovimento DATETIME NOT NULL,
                tipomovimento CHAR(1) NOT NULL,
                valor DECIMAL(18, 2) NOT NULL
            );
        ");

            _repository = new MovimentoRepository(_unitOfWork, _connection);
        }

        [Fact]
        public async Task AddMovimento_ShouldAddMovimento()
        {
            var request = new AddMovimentoRequest
            {
                IdMovimento = Guid.NewGuid().ToString(),
                IdContaCorrente = "123456",
                DataMovimento = DateTime.Now,
                TipoMovimento = 'C',
                Valor = 100.00m
            };

            var response = await _repository.AddMovimento(request);

            Assert.NotNull(response);
            Assert.Equal(request.IdMovimento, response.IdMovimento);

            var movimento = await _repository.GetMovimentoById(request.IdMovimento);
            Assert.NotNull(movimento);
            Assert.Equal(request.IdContaCorrente, movimento.IdContaCorrente);
            Assert.Equal(request.TipoMovimento, movimento.TipoMovimento);
            Assert.Equal(request.Valor, movimento.Valor);
        }

        [Fact]
        public async Task GetMovimentoById_ShouldReturnMovimento()
        {
            var idMovimento = Guid.NewGuid().ToString();
            var movimento = new Movimento(idMovimento, "123456", DateTime.Now.AddMinutes(1), 'D', 50.00m);

            await _connection.ExecuteAsync(@"
            INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor)
            VALUES (@IdMovimento, @IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor);
        ", movimento);

            var result = await _repository.GetMovimentoById(idMovimento);

            Assert.NotNull(result);
            Assert.Equal(idMovimento, result.IdMovimento);
        }

        [Fact]
        public async Task UpdateMovimento_ShouldUpdateMovimento()
        {
            var idMovimento = Guid.NewGuid().ToString();
            var movimento = new Movimento(idMovimento, "123456", DateTime.Now.AddMinutes(1), 'D', 50.00m);

            await _connection.ExecuteAsync(@"
            INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor)
            VALUES (@IdMovimento, @IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor);
        ", movimento);

            var updateRequest = new UpdateMovimentoRequest
            {
                IdMovimento = idMovimento,
                IdContaCorrente = "654321",
                DataMovimento = movimento.DataMovimento,
                TipoMovimento = 'C',
                Valor = 150.00m
            };

            await _repository.UpdateMovimento(updateRequest);

            var updatedMovimento = await _repository.GetMovimentoById(idMovimento);

            Assert.NotNull(updatedMovimento);
            Assert.NotEqual(updateRequest.IdContaCorrente, updatedMovimento.IdContaCorrente);
            Assert.NotEqual(updateRequest.TipoMovimento, updatedMovimento.TipoMovimento);
            Assert.NotEqual(updateRequest.Valor, updatedMovimento.Valor);
        }

        [Fact]
        public async Task DeleteMovimento_ShouldDeleteMovimento()
        {
            var idMovimento = Guid.NewGuid().ToString();
            var movimento = new Movimento(idMovimento, "123456", DateTime.Now.AddMinutes(1), 'D', 50.00m);

            await _connection.ExecuteAsync(@"
            INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor)
            VALUES (@IdMovimento, @IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor);
        ", movimento);

            await _repository.DeleteMovimento(new DeleteMovimentoRequest { IdMovimento = idMovimento });

            var deletedMovimento = await _repository.GetMovimentoById(idMovimento);
            Assert.Null(deletedMovimento);
        }

        [Fact]
        public async Task GetAllMovimentos_ShouldReturnAllMovimentos()
        {
            var movimento1 = new Movimento("123456", 'C', 100.00m);
            var movimento2 = new Movimento("789012", 'D', 50.00m);

            await _connection.ExecuteAsync(@"
            INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor)
            VALUES (@IdMovimento, @IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor);
        ", movimento1);

            await _connection.ExecuteAsync(@"
            INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor)
            VALUES (@IdMovimento, @IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor);
        ", movimento2);

            var movimentos = await _repository.GetAllMovimentos();

            Assert.NotNull(movimentos);
            Assert.Equal(2, movimentos.Count());
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
            _connection.Dispose();
        }
    }
}
