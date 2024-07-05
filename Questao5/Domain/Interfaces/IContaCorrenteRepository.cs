using Questao5.Domain.Entities;

namespace Questao5.Domain.Interfaces
{
    public interface IContaCorrenteRepository
    {
        Task<IEnumerable<ContaCorrente>> GetAllContasCorrente();
        Task<ContaCorrente> GetContaCorrenteById(string idContaCorrente);

        Task<ContaCorrente> AddContCorrente(ContaCorrente contaCorrente);
        void UpdateContCorrente(ContaCorrente contaCorrente);
        Task<ContaCorrente> DeleteContCorrente(string idContaCorrente);
    }
}
