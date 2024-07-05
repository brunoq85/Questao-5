using Dapper;
using Questao5.Domain.Entities;
using Questao5.Domain.Interfaces;
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

        public async Task<IEnumerable<ContaCorrente>> GetAllContasCorrente()
        {
            var query = "SELECT * FROM contacorrente";
            return await _connection.QueryAsync<ContaCorrente>(query, null, _unitOfWork.Transaction);

        }

        public async Task<ContaCorrente> GetContaCorrenteById(string idContaCorrente)
        {
            var query = "SELECT * FROM contacorrente WHERE idcontacorrente = @idContaCorrente";
            return await _connection.QuerySingleOrDefaultAsync<ContaCorrente>(query, new { idContaCorrente = idContaCorrente }, _unitOfWork.Transaction);
        }

        public async Task<ContaCorrente> AddContCorrente(ContaCorrente contaCorrente)
        {
            if (contaCorrente is null)
                throw new ArgumentNullException(nameof(contaCorrente));

            var query = "INSERT INTO contacorrente (idcontacorrente, numero, nome, ativo) VALUES (@IdContaCorrente, @Numero, @Nome, @Ativo)";

            await _connection.ExecuteAsync(query, contaCorrente, _unitOfWork.Transaction);

            return contaCorrente;
        }

        public async Task<ContaCorrente> DeleteContCorrente(string idContaCorrente)
        {
            var querySelect = "SELECT * FROM contacorrente WHERE idcontacorrente = @idContaCorrente";
            var queryDelete = "DELETE FROM contacorrente WHERE idcontacorrente = @idContaCorrente";

            // Obter a conta corrente antes de deletar
            var contaCorrente = await _connection.QuerySingleOrDefaultAsync<ContaCorrente>(querySelect, new { idContaCorrente = idContaCorrente });

            if (contaCorrente == null)
            {
                return null;
            }

            // Deletar a conta corrente
            await _connection.ExecuteAsync(queryDelete, new { idContaCorrente = idContaCorrente }, _unitOfWork.Transaction);

            return contaCorrente;
        }

        public async void UpdateContCorrente(ContaCorrente contaCorrente)
        {
            var query = "UPDATE contacorrente SET numero = @Numero, nome = @Nome, ativo = @Ativo WHERE idcontacorrente = @IdContaCorrente";
            await _connection.ExecuteAsync(query, contaCorrente, _unitOfWork.Transaction);
        }
    }
}
