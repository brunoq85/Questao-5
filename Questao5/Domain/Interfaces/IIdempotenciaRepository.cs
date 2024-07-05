using Questao5.Domain.Entities;

namespace Questao5.Domain.Interfaces
{
    public interface IIdempotenciaRepository
    {
        Task<bool> VerificarChaveExistenteAsync(string chaveIdempotencia);

        Task SalvarResultadoAsync(string chaveIdempotencia, string resultado);
    }
}
