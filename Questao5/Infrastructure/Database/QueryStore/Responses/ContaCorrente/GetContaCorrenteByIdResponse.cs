namespace Questao5.Infrastructure.Database.QueryStore.Responses.ContaCorrente
{
    public class GetContaCorrenteByIdResponse
    {
        public string IdContaCorrente { get; set; }
        public int Numero { get; set; }
        public string Nome { get; set; }
        public bool Ativo { get; set; }
    }
}