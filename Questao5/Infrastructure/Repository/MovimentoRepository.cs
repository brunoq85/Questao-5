using Dapper;
using Questao5.Domain.Entities;
using Questao5.Domain.Interfaces;
using System.Data;

namespace Questao5.Infrastructure.Repository
{
    public class MovimentoRepository : IMovimentoRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDbConnection _connection;

        public MovimentoRepository(IUnitOfWork unitOfWork, IDbConnection connection)
        {
            _unitOfWork = unitOfWork;
            _connection = connection;
        }

        public async Task<IEnumerable<Movimento>> GetAllMovimentos()
        {
            var query = "SELECT * FROM movimento";
            return await _connection.QueryAsync<Movimento>(query, null, _unitOfWork.Transaction);
        }

        public async Task<Movimento> GetMovimentoById(string idMovimento)
        {
            var query = "SELECT * FROM movimento WHERE idmovimento = @idmovimento";
            return await _connection.QuerySingleOrDefaultAsync<Movimento>(query, new { idmovimento = idMovimento }, _unitOfWork.Transaction);
        }

        public async Task<IEnumerable<Movimento>> GetMovimentoByIdContaCorrente(string idContaCorrente)
        {
            var query = "SELECT * FROM movimento WHERE idcontacorrente = @idcontacorrente";
            return await _connection.QueryAsync<Movimento>(query, new { idcontacorrente = idContaCorrente }, _unitOfWork.Transaction);
        }

        public async Task<Movimento> AddMovimento(Movimento movimento)
        {
            if (movimento is null)
                throw new ArgumentNullException(nameof(movimento));

            var query = @"INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor) 
                          VALUES (@IdMovimento, @IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor)";

            await _connection.ExecuteAsync(query, movimento, _unitOfWork.Transaction);

            return movimento;
        }

        public async Task<Movimento> DeleteMovimento(string idMovimento)
        {
            var querySelect = "SELECT * FROM movimento WHERE idmovimento = @idMovimento";
            var queryDelete = "DELETE FROM movimento WHERE idMovimento = @idMovimento";


            // Obter a conta corrente antes de deletar
            var movimento = await _connection.QuerySingleOrDefaultAsync<Movimento>(querySelect, new { idMovimento = idMovimento }, _unitOfWork.Transaction);

            if (movimento == null)
            {
                return null;
            }

            // Deletar a conta corrente
            await _connection.ExecuteAsync(queryDelete, new { idMovimento = idMovimento });

            return movimento;

        }

        public async void UpdateMovimento(Movimento movimento)
        {
            var query = "UPDATE contacorrente SET idcontacorrente = @IdContaCorrente, datamovimento = @DataMovimento, tipomovimento = @TipoMovimento, valor = @Valor WHERE idmovimento = @IdMovimento";
            await _connection.ExecuteAsync(query, movimento, _unitOfWork.Transaction);
        }
    }
}
