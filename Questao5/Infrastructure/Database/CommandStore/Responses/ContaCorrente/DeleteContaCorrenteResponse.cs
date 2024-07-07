namespace Questao5.Infrastructure.Database.CommandStore.Responses.ContaCorrente
{
    public class DeleteContaCorrenteResponse
    {
        public string IdContaCorrente { get; set; }
        public int Numero { get; set; }
        public string Nome { get; set; }
        public bool Ativo { get; set; }
    }
}
