namespace Questao5.Infrastructure.Database.CommandStore.Requests.Movimento
{
    public class UpdateMovimentoRequest
    {
        public string IdMovimento { get; set; }
        public string IdContaCorrente { get; set; }
        public DateTime DataMovimento { get; set; }
        public char TipoMovimento { get; set; }
        public decimal Valor { get; set; }
    }
}
