using FluentAssertions;
using Microsoft.Data.Sqlite;
using Questao5.Application.ContaCorrente.Queries;
using Questao5.Application.ContaCorrente.Queries.Handler;
using Questao5.Domain.Interfaces;
using Questao5.Infrastructure.Database.CommandStore.Requests.ContaCorrente;
using Questao5.Infrastructure.Database.CommandStore.Requests.Movimento;
using Questao5.Infrastructure.Repository;
using System.Data;
using Xunit;

namespace Questao5.Tests.Integracao
{
    public class GetMovimentoByIdQueryHandlerTests
    {
        private readonly IDbConnection _connection;
        private readonly IMovimentoRepository _movimentoRepository;
        private readonly GetMovimentoByIdQueryHandler _handler;
        private readonly UnitOfWork _unitOfWork;

        public GetMovimentoByIdQueryHandlerTests()
        {
            // Configurar o banco de dados de teste, repositório e handler aqui
            _connection = new SqliteConnection("Data Source=:memory:");
            _connection.Open();

            _unitOfWork = new UnitOfWork(_connection);

            using (var cmd = _connection.CreateCommand())
            {
                // Primeiro, crie a tabela contacorrente
                cmd.CommandText = @"
            CREATE TABLE contacorrente (
                idcontacorrente TEXT(37) PRIMARY KEY, -- id da conta corrente
                numero INTEGER(10) NOT NULL UNIQUE, -- numero da conta corrente
                nome TEXT(100) NOT NULL, -- nome do titular da conta corrente
                ativo INTEGER(1) NOT NULL default 0, -- indicativo se a conta esta ativa. (0 = inativa, 1 = ativa).
                CHECK (ativo in (0,1))
            )";
                cmd.ExecuteNonQuery();

                // Em seguida, crie a tabela movimento
                cmd.CommandText = @"
            CREATE TABLE movimento (
                idmovimento TEXT(37) PRIMARY KEY, -- identificacao unica do movimento
                idcontacorrente TEXT(37) NOT NULL, -- identificacao unica da conta corrente
                datamovimento TEXT(25) NOT NULL, -- data do movimento no formato DD/MM/YYYY
                tipomovimento TEXT(1) NOT NULL, -- tipo do movimento. (C = Credito, D = Debito).
                valor REAL NOT NULL, -- valor do movimento. Usar duas casas decimais.
                CHECK (tipomovimento in ('C','D')),
                FOREIGN KEY(idcontacorrente) REFERENCES contacorrente(idcontacorrente)
            )";
                cmd.ExecuteNonQuery();
            }

            _movimentoRepository = new MovimentoRepository(_unitOfWork, _connection);
            _handler = new GetMovimentoByIdQueryHandler(_movimentoRepository);
        }

        [Fact]
        public async Task Handle_ShouldReturnMovimentoByIdContaCorrente()
        {
            // Arrange
            var request = new AddContaCorrenteRequest
            {
                IdContaCorrente = "B6BAFC09-6967-ED11-A567-055DFA4A16C9",
                Numero = 123,
                Nome = "Katherine Sanchez",
                Ativo = true
            };

            var contaCorrenteRepository = new Questao5.Infrastructure.Repository.ContaCorrenteRepository(_unitOfWork, _connection);

            await contaCorrenteRepository.AddContaCorrente(request);

            var idContaCorrente = "B6BAFC09-6967-ED11-A567-055DFA4A16C9";
            var addMovimentoRequest = new AddMovimentoRequest
            {
                IdMovimento = Guid.NewGuid().ToString(),
                IdContaCorrente = idContaCorrente,
                DataMovimento = DateTime.Now.AddMinutes(5),
                TipoMovimento = 'C',
                Valor = 100
            };
            await _movimentoRepository.AddMovimento(addMovimentoRequest);

            var query = new GetMovimentoByIdQuery(idContaCorrente);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCountGreaterThan(0);
            result.First().IdContaCorrente.Should().Be(idContaCorrente);
        }
    }

}
