namespace Questao5.Infrastructure.Database.CommandStore.Responses.Movimento
{
    public class DeleteMovimentoResponse
    {
        public string IdMovimento { get; set; }
        public string IdContaCorrente { get; set; }
        public DateTime DataMovimento { get; set; }
        public char TipoMovimento { get; set; }
        public decimal Valor { get; set; }
    }
}
