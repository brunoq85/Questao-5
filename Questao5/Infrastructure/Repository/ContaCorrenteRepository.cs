using Dapper;
using Questao5.Domain.Entities;
using Questao5.Domain.Interfaces;
using Questao5.Infrastructure.Database.CommandStore.Requests.ContaCorrente;
using Questao5.Infrastructure.Database.CommandStore.Responses.ContaCorrente;
using Questao5.Infrastructure.Database.QueryStore.Requests.ContaCorrente;
using Questao5.Infrastructure.Database.QueryStore.Responses.ContaCorrente;
using System.Data;

namespace Questao5.Infrastructure.Repository
{
    public class ContaCorrenteRepository : IContaCorrenteRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDbConnection _connection;

        public ContaCorrenteRepository(IUnitOfWork unitOfWork, IDbConnection connection)
        {
            _unitOfWork = unitOfWork;
            _connection = connection;
        }

        public async Task<IEnumerable<GetContaCorrenteByIdResponse>> GetAllContasCorrente()
        {
            var query = "SELECT * FROM contacorrente";
            return await _connection.QueryAsync<GetContaCorrenteByIdResponse>(query, null, _unitOfWork.Transaction);

        }

        public async Task<GetContaCorrenteByIdResponse> GetContaCorrenteById(GetContaCorrenteByIdRequest getContaCorrenteByIdRequest)
        {
            var query = "SELECT * FROM contacorrente WHERE idcontacorrente = @IdContaCorrente";

            var result = await _connection.QueryFirstAsync<GetContaCorrenteByIdResponse>(query, new { IdContaCorrente = getContaCorrenteByIdRequest.IdContaCorrente });

            return result;
        }

        public async Task<AddContaCorrenteResponse> AddContaCorrente(AddContaCorrenteRequest addContaCorrenteRequest)
        {
            if (addContaCorrenteRequest is null)
                throw new ArgumentNullException(nameof(addContaCorrenteRequest));

            var query = "INSERT INTO contacorrente (idcontacorrente, numero, nome, ativo) VALUES (@IdContaCorrente, @Numero, @Nome, @Ativo)";

            await _connection.ExecuteAsync(query, addContaCorrenteRequest, _unitOfWork.Transaction);

            return new AddContaCorrenteResponse
            {
                IdContaCorrente = addContaCorrenteRequest.IdContaCorrente,
                Numero = addContaCorrenteRequest.Numero,
                Nome = addContaCorrenteRequest.Nome,
                Ativo = addContaCorrenteRequest.Ativo
            };
        }

        public async Task<DeleteContaCorrenteResponse> DeleteContaCorrente(DeleteContaCorrenteRequest deleteContaCorrenteRequest)
        {
            var querySelect = "SELECT * FROM contacorrente WHERE idcontacorrente = @idContaCorrente";
            var queryDelete = "DELETE FROM contacorrente WHERE idcontacorrente = @idContaCorrente";

            // Obter a conta corrente antes de deletar
            var contaCorrente = await _connection.QuerySingleOrDefaultAsync<ContaCorrente>(querySelect, new { idContaCorrente = deleteContaCorrenteRequest.IdContaCorrente });

            if (contaCorrente == null)
            {
                return null;
            }

            // Deletar a conta corrente
            await _connection.ExecuteAsync(queryDelete, new { idContaCorrente = deleteContaCorrenteRequest.IdContaCorrente }, _unitOfWork.Transaction);

            return new DeleteContaCorrenteResponse
            {
                IdContaCorrente = contaCorrente.IdContaCorrente,
                Numero = contaCorrente.Numero,
                Nome = contaCorrente.Nome,
                Ativo = contaCorrente.Ativo
            };
        }

        public async Task<UpdateContaCorrenteResponse> UpdateContaCorrente(UpdateContaCorrenteRequest updateContaCorrenteRequest)
        {
            var query = "UPDATE contacorrente SET numero = @Numero, nome = @Nome, ativo = @Ativo WHERE idcontacorrente = @IdContaCorrente";
            await _connection.ExecuteAsync(query, updateContaCorrenteRequest, _unitOfWork.Transaction);

            return new UpdateContaCorrenteResponse
            {
                IdContaCorrente = updateContaCorrenteRequest.IdContaCorrente,
                Numero = updateContaCorrenteRequest.Numero,
                Nome = updateContaCorrenteRequest.Nome,
                Ativo = updateContaCorrenteRequest.Ativo,
            };
        }
    }
}
