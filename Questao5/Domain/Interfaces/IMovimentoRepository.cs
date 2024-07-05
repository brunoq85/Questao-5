using Questao5.Domain.Entities;

namespace Questao5.Domain.Interfaces
{
    public interface IMovimentoRepository
    {
        Task<IEnumerable<Movimento>> GetAllMovimentos();
        Task<Movimento> GetMovimentoById(string idMovimento);
        Task<IEnumerable<Movimento>> GetMovimentoByIdContaCorrente(string idContaCorrente);

        Task<Movimento> AddMovimento(Movimento movimento);
        void UpdateMovimento(Movimento movimento);
        Task<Movimento> DeleteMovimento(string idMovimento);
    }
}
