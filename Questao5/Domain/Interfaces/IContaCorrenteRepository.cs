using Questao5.Infrastructure.Database.CommandStore.Requests.ContaCorrente;
using Questao5.Infrastructure.Database.CommandStore.Responses.ContaCorrente;
using Questao5.Infrastructure.Database.QueryStore.Requests.ContaCorrente;
using Questao5.Infrastructure.Database.QueryStore.Responses.ContaCorrente;

namespace Questao5.Domain.Interfaces
{
    public interface IContaCorrenteRepository
    {
        Task<IEnumerable<GetContaCorrenteByIdResponse>> GetAllContasCorrente();
        Task<GetContaCorrenteByIdResponse> GetContaCorrenteById(GetContaCorrenteByIdRequest getContaCorrenteByIdRequest);

        Task<AddContaCorrenteResponse> AddContaCorrente(AddContaCorrenteRequest addContaCorrenteRequest);
        Task<DeleteContaCorrenteResponse> DeleteContaCorrente(DeleteContaCorrenteRequest deleteContaCorrenteRequest);
        Task<UpdateContaCorrenteResponse> UpdateContaCorrente(UpdateContaCorrenteRequest updateContaCorrenteRequest);
    }
}
