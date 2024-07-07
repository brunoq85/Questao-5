using MediatR;
using Questao5.Domain.Interfaces;
using Questao5.Infrastructure.Database.CommandStore.Requests.Movimento;
using Questao5.Infrastructure.Database.CommandStore.Responses.Movimento;

namespace Questao5.Application.Movimento.Commands.Handler
{
    public class AddMovimentoCommandHandler : IRequestHandler<AddMovimentoCommand, AddMovimentoResponse>
    {
        private readonly IMovimentoRepository _movimentoRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddMovimentoCommandHandler(IMovimentoRepository movimentoRepository, IUnitOfWork unitOfWork)
        {
            _movimentoRepository = movimentoRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<AddMovimentoResponse> Handle(AddMovimentoCommand request, CancellationToken cancellationToken)
        {
            var movimento = new Domain.Entities.Movimento(request.IdContaCorrente, request.TipoMovimento, request.Valor);

            var result = await _movimentoRepository.AddMovimento(
                new AddMovimentoRequest
                {
                    IdMovimento = movimento.IdMovimento,
                    IdContaCorrente = movimento.IdContaCorrente,
                    DataMovimento = movimento.DataMovimento,
                    TipoMovimento = movimento.TipoMovimento,
                    Valor = movimento.Valor,
                });
            //await _unitOfWork.CommitAsync();

            return new AddMovimentoResponse
            {
                IdMovimento = result.IdMovimento,
            };
        }
    }
}
