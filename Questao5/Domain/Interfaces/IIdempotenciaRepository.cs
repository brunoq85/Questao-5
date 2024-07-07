namespace Questao5.Domain.Interfaces
{
    public interface IIdempotenciaRepository
    {
        Task<bool> VerificarChaveExistenteAsync(string chaveIdempotencia);
        Task RegistrarChaveAsync(string chaveIdempotencia, string requisicao);
        Task AtualizarResultadoAsync(string chaveIdempotencia, string resultado);
        Task<string> ObterResultadoPorChaveAsync(string chaveIdempotencia);
    }
}
