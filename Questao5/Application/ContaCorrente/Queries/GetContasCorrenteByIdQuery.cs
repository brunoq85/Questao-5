using MediatR;
using Questao5.Application.ContaCorrente.Queries;
using Questao5.Infrastructure.Database.QueryStore.Responses.ContaCorrente;
using Questao5.Infrastructure.Repository;

namespace Questao5.Application.ContaCorrente.Queries
{
    public class GetContasCorrenteByIdQuery : IRequest<GetContaCorrenteByIdResponse>
    {
        public GetContasCorrenteByIdQuery(string idContaCorrente)
        {
            IdContaCorrente = idContaCorrente;
        }

        public string IdContaCorrente { get; set; }
    }
}