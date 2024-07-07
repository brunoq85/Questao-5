using MediatR;
using Questao5.Infrastructure.Database.QueryStore.Responses.Movimento;

namespace Questao5.Application.ContaCorrente.Queries
{
    public class GetMovimentoByIdQuery : IRequest<IEnumerable< GetMovimentoByIdResponse>>
    {
        public GetMovimentoByIdQuery(string idContaCorrente)
        {
            IdContaCorrente = idContaCorrente;
        }

        public string IdMovimento { get; set; }
        public string IdContaCorrente { get; set; }
        public DateTime DataMovimento { get; set; }
        public char TipoMovimento { get; set; }
        public decimal Valor { get; set; }
    }
}