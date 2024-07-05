namespace Questao5.Application.Commands.Requests
{
    public class MovimentoRequest
    {
        public string IdContaCorrente { get; set; }
        public decimal Valor { get; set; }
        public char TipoMovimento { get; set; }
    }
}
