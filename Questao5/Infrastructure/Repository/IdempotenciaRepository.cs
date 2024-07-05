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

        public async Task<bool> VerificarChaveExistenteAsync(string chaveIdempotencia)
        {
            var query = $"SELECT CASE WHEN COUNT(*) > 0 THEN 1 ELSE 0 END AS Existe FROM idempotencia WHERE chave_idempotencia = @ChaveIdempotencia";
            var result = await _connection.QuerySingleAsync<bool>(query, new { ChaveIdempotencia = chaveIdempotencia });
            return result;
        }

        public async Task SalvarResultadoAsync(string chaveIdempotencia, string resultado)
        {
            var idempotencia = new Idempotencia(chaveIdempotencia, "", resultado);


            var query = @"INSERT INTO idempotencia (chave_idempotencia, requisicao, resultado) 
                          VALUES (@ChaveIdempotencia, @Requisicao, @Resultado)";

            await _connection.ExecuteAsync(query, idempotencia, _unitOfWork.Transaction);
        }
    }
}
