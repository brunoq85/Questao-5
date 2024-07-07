using MediatR;
using Questao5.Domain.Interfaces;
using Questao5.Infrastructure.Database.QueryStore.Requests.ContaCorrente;
using Questao5.Infrastructure.Database.QueryStore.Responses.ContaCorrente;

namespace Questao5.Application.ContaCorrente.Queries.Handler
{
    public class GetContasCorrenteByIdQueryHandler : IRequestHandler<GetContasCorrenteByIdQuery, GetContaCorrenteByIdResponse>
    {
        private readonly IContaCorrenteRepository _contaCorrenteRepository;

        public GetContasCorrenteByIdQueryHandler(IContaCorrenteRepository contaCorrenteRepository)
        {
            _contaCorrenteRepository = contaCorrenteRepository;
        }

        public async Task<GetContaCorrenteByIdResponse> Handle(GetContasCorrenteByIdQuery request, CancellationToken cancellationToken)
        {
            var contaCorrente = await _contaCorrenteRepository.GetContaCorrenteById(new GetContaCorrenteByIdRequest { IdContaCorrente = request.IdContaCorrente });

            return contaCorrente;
        }
    }
}