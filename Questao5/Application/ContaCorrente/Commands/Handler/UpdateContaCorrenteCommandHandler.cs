using MediatR;
using Questao5.Domain.Interfaces;
using Questao5.Infrastructure.Database.CommandStore.Requests.ContaCorrente;
using Questao5.Infrastructure.Database.CommandStore.Responses.ContaCorrente;

namespace Questao5.Application.ContaCorrente.Commands.Handler
{
    public class UpdateContaCorrenteCommandHandler : IRequestHandler<UpdateContaCorrenteCommand, UpdateContaCorrenteResponse>
    {
        private readonly IContaCorrenteRepository _contaCorrenteRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateContaCorrenteCommandHandler(IContaCorrenteRepository contaCorrenteRepository, IUnitOfWork unitOfWork)
        {
            _contaCorrenteRepository = contaCorrenteRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<UpdateContaCorrenteResponse> Handle(UpdateContaCorrenteCommand request, CancellationToken cancellationToken)
        {
            var contaCorrente = new Domain.Entities.ContaCorrente(request.IdContaCorrente, request.Numero, request.Nome, request.Ativo);

            await _contaCorrenteRepository.UpdateContaCorrente(
                new UpdateContaCorrenteRequest
                {
                    IdContaCorrente = contaCorrente.IdContaCorrente,
                    Numero = contaCorrente.Numero,
                    Nome = contaCorrente.Nome,
                    Ativo = contaCorrente.Ativo,
                });

            await _unitOfWork.CommitAsync();

            return new UpdateContaCorrenteResponse
            {
                IdContaCorrente = contaCorrente.IdContaCorrente,
                Numero = contaCorrente.Numero,
                Nome = contaCorrente.Nome,
                Ativo = contaCorrente.Ativo,
            };
        }
    }
}
