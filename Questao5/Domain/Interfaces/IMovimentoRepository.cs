using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database.CommandStore.Requests.Movimento;
using Questao5.Infrastructure.Database.CommandStore.Responses.Movimento;
using Questao5.Infrastructure.Database.QueryStore.Responses.Movimento;

namespace Questao5.Domain.Interfaces
{
    public interface IMovimentoRepository
    {
        Task<IEnumerable<Movimento>> GetAllMovimentos();
        Task<Movimento> GetMovimentoById(string idMovimento);
        Task<IEnumerable<GetMovimentoByIdResponse>> GetMovimentoByIdContaCorrente(string idContaCorrente);

        Task<AddMovimentoResponse> AddMovimento(AddMovimentoRequest addMovimentoRequest);
        Task<UpdateMovimentoResponse> UpdateMovimento(UpdateMovimentoRequest updateMovimentoRequest);
        Task<DeleteMovimentoResponse> DeleteMovimento(DeleteMovimentoRequest deleteMovimentoRequest);
    }
}
