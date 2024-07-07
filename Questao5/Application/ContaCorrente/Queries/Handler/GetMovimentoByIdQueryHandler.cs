using MediatR;
using Questao5.Domain.Interfaces;
using Questao5.Infrastructure.Database.QueryStore.Responses.Movimento;

namespace Questao5.Application.ContaCorrente.Queries.Handler
{
    public class GetMovimentoByIdQueryHandler : IRequestHandler<GetMovimentoByIdQuery, IEnumerable<GetMovimentoByIdResponse>>
    {
        private readonly IMovimentoRepository _movimentoRepository;

        public GetMovimentoByIdQueryHandler(IMovimentoRepository movimentoRepository)
        {
            _movimentoRepository = movimentoRepository;
        }

        public async Task<IEnumerable<GetMovimentoByIdResponse>> Handle(GetMovimentoByIdQuery request, CancellationToken cancellationToken)
        {
            var movimentos = await _movimentoRepository.GetMovimentoByIdContaCorrente(request.IdContaCorrente);

            return movimentos;
        }
    }
}