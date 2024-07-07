using Questao5.Domain.Validation;

namespace Questao5.Domain.Entities
{
    public class Idempotencia
    {
        public string ChaveIdempotencia { get; private set; } = null!;
        public string Requisicao { get; private set; } = null!;
        public string Resultado { get; private set; } = null!;

        public Idempotencia(string chaveIdempotencia, string requisicao, string resultado)
        {
            if (chaveIdempotencia.Length > 37)
                throw new Exception("A chave deve possui até 37 caracteres.");

            ChaveIdempotencia = Guid.NewGuid().ToString();
            Requisicao = requisicao;
            Resultado = resultado;
        }
    }
}
