using Dapper;
using Questao5.Domain.Entities;
using Questao5.Domain.Interfaces;
using Questao5.Infrastructure.Database.CommandStore.Requests.Movimento;
using Questao5.Infrastructure.Database.CommandStore.Responses.Movimento;
using Questao5.Infrastructure.Database.QueryStore.Responses.Movimento;
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

        public async Task<IEnumerable<GetMovimentoByIdResponse>> GetMovimentoByIdContaCorrente(string idContaCorrente)
        {
            var query = "SELECT * FROM movimento WHERE idcontacorrente = @idcontacorrente";
            return await _connection.QueryAsync<GetMovimentoByIdResponse>(query, new { idcontacorrente = idContaCorrente }, _unitOfWork.Transaction);
        }

        public async Task<AddMovimentoResponse> AddMovimento(AddMovimentoRequest addMovimentoRequest)
        {
            if (addMovimentoRequest is null)
                throw new ArgumentNullException(nameof(addMovimentoRequest));

            var query = @"INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor) 
                          VALUES (@IdMovimento, @IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor)";

            await _connection.ExecuteAsync(query, new
            {
                IdMovimento = addMovimentoRequest.IdMovimento,
                IdContaCorrente = addMovimentoRequest.IdContaCorrente,
                DataMovimento = addMovimentoRequest.DataMovimento,
                TipoMovimento = addMovimentoRequest.TipoMovimento,
                Valor = addMovimentoRequest.Valor
            }, _unitOfWork.Transaction);

            return new AddMovimentoResponse
            {
                IdMovimento = addMovimentoRequest.IdMovimento,
            };
        }

        public async Task<DeleteMovimentoResponse> DeleteMovimento(DeleteMovimentoRequest deleteMovimentoRequest)
        {
            var querySelect = "SELECT * FROM movimento WHERE idmovimento = @idMovimento";
            var queryDelete = "DELETE FROM movimento WHERE idMovimento = @idMovimento";


            // Obter a conta corrente antes de deletar
            var movimento = await _connection.QuerySingleOrDefaultAsync<Movimento>(querySelect, new { idMovimento = deleteMovimentoRequest.IdMovimento }, _unitOfWork.Transaction);

            if (movimento == null)
            {
                return null;
            }

            // Deletar a conta corrente
            await _connection.ExecuteAsync(queryDelete, new { idMovimento = movimento.IdMovimento }, _unitOfWork.Transaction);

            return new DeleteMovimentoResponse
            {
                IdMovimento = movimento.IdMovimento,
                IdContaCorrente = movimento.IdMovimento,
                DataMovimento = movimento.DataMovimento,
                TipoMovimento = movimento.TipoMovimento,
                Valor = movimento.Valor
            };

        }

        public async Task<UpdateMovimentoResponse> UpdateMovimento(UpdateMovimentoRequest updateMovimentoRequest)
        {
            var querySelect = "SELECT * FROM movimento WHERE idmovimento = @IdMovimento";
            var queryUpdate = @"UPDATE movimento 
SET idcontacorrente = @IdContaCorrente, 
datamovimento = @DataMovimento, 
tipomovimento = @TipoMovimento, 
valor = @Valor 
WHERE idmovimento = @IdMovimento";

            var movimento = await _connection.QueryFirstOrDefaultAsync<UpdateMovimentoResponse>(querySelect,
                new
                {
                    IdMovimento = updateMovimentoRequest.IdMovimento
                }, _unitOfWork.Transaction);

            if (movimento == null)
            {
                return null;
            }

            await _connection.ExecuteAsync(queryUpdate,
                new
                {
                    IdMovimento = movimento.IdMovimento,
                    IdContaCorrente = movimento.IdContaCorrente,
                    DataMovimento = movimento.DataMovimento,
                    TipoMovimento = movimento.TipoMovimento,
                    Valor = movimento.Valor
                }, _unitOfWork.Transaction);

            return new UpdateMovimentoResponse
            {
                IdMovimento = movimento.IdMovimento,
                IdContaCorrente = movimento.IdMovimento,
                DataMovimento = movimento.DataMovimento,
                TipoMovimento = movimento.TipoMovimento,
                Valor = movimento.Valor
            };
        }
    }
}
