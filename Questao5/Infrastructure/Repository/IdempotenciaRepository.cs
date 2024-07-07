using Dapper;
using NSubstitute;
using Questao5.Domain.Entities;
using Questao5.Domain.Interfaces;
using System.Data;

namespace Questao5.Infrastructure.Repository
{
    public class IdempotenciaRepository : IIdempotenciaRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDbConnection _connection;

        public IdempotenciaRepository(IUnitOfWork unitOfWork, IDbConnection connection)
        {
            _unitOfWork = unitOfWork;
            _connection = connection;
        }

        // Verifica se a chave de idempotência já existe na tabela
        public async Task<bool> VerificarChaveExistenteAsync(string chaveIdempotencia)
        {
            var query = "SELECT COUNT(1) FROM idempotencia WHERE chave_idempotencia = @ChaveIdempotencia";
            var count = await _connection.ExecuteScalarAsync<int>(query, new { ChaveIdempotencia = chaveIdempotencia }, _unitOfWork.Transaction);
            return count > 0;
        }

        // Registra uma nova chave de idempotência na tabela
        public async Task RegistrarChaveAsync(string chaveIdempotencia, string requisicao)
        {
            var query = "INSERT INTO idempotencia (chave_idempotencia, requisicao) VALUES (@ChaveIdempotencia, @Requisicao)";
            await _connection.ExecuteAsync(query, new { ChaveIdempotencia = chaveIdempotencia, Requisicao = requisicao }, _unitOfWork.Transaction);
        }

        // Atualiza o resultado associado a uma chave de idempotência
        public async Task AtualizarResultadoAsync(string chaveIdempotencia, string resultado)
        {
            var query = "UPDATE idempotencia SET resultado = @Resultado WHERE chave_idempotencia = @ChaveIdempotencia";
            await _connection.ExecuteAsync(query, new { Resultado = resultado, ChaveIdempotencia = chaveIdempotencia }, _unitOfWork.Transaction);
        }

        // Obtém o resultado associado a uma chave de idempotência
        public async Task<string> ObterResultadoPorChaveAsync(string chaveIdempotencia)
        {
            var query = "SELECT resultado FROM idempotencia WHERE chave_idempotencia = @ChaveIdempotencia";
            var resultado = await _connection.QuerySingleOrDefaultAsync<string>(query, new { ChaveIdempotencia = chaveIdempotencia }, _unitOfWork.Transaction);
            return resultado;
        }
    }
}
