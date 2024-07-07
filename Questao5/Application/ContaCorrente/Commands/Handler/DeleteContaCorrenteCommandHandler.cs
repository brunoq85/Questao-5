using MediatR;
using Questao5.Application.ContaCorrente.Commands;
using Questao5.Domain.Interfaces;
using Questao5.Infrastructure.Database.CommandStore.Requests.ContaCorrente;
using Questao5.Infrastructure.Database.CommandStore.Responses.ContaCorrente;

namespace Questao5.Application.ContaCorrente.Commands.Handler
{
    public class DeleteContaCorrenteCommandHandler : IRequestHandler<DeleteContaCorrenteCommand, DeleteContaCorrenteResponse>
    {
        private readonly IContaCorrenteRepository _contaCorrenteRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteContaCorrenteCommandHandler(IContaCorrenteRepository contaCorrenteRepository, IUnitOfWork unitOfWork)
        {
            _contaCorrenteRepository = contaCorrenteRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteContaCorrenteResponse> Handle(DeleteContaCorrenteCommand request, CancellationToken cancellationToken)
        {
            var contaCorrente = new Domain.Entities.ContaCorrente(request.IdContaCorrente, request.Numero, request.Nome, request.Ativo);

            await _contaCorrenteRepository.DeleteContaCorrente(
                new DeleteContaCorrenteRequest
                {
                    IdContaCorrente = contaCorrente.IdContaCorrente,
                });

            await _unitOfWork.CommitAsync();

            return new DeleteContaCorrenteResponse
            {
                IdContaCorrente = contaCorrente.IdContaCorrente,
                Numero = contaCorrente.Numero,
                Nome = contaCorrente.Nome,
                Ativo = contaCorrente.Ativo,
            };
        }
    }
}
