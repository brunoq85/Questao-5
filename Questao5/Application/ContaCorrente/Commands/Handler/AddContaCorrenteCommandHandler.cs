using MediatR;
using Questao5.Application.ContaCorrente.Commands;
using Questao5.Domain.Interfaces;
using Questao5.Infrastructure.Database.CommandStore.Requests.ContaCorrente;
using Questao5.Infrastructure.Database.CommandStore.Responses.ContaCorrente;

namespace Questao5.Application.ContaCorrente.Commands.Handler
{
    public class AddContaCorrenteCommandHandler : IRequestHandler<AddContaCorrenteCommand, AddContaCorrenteResponse>
    {
        private readonly IContaCorrenteRepository _contaCorrenteRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddContaCorrenteCommandHandler(IContaCorrenteRepository contaCorrenteRepository, IUnitOfWork unitOfWork)
        {
            _contaCorrenteRepository = contaCorrenteRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<AddContaCorrenteResponse> Handle(AddContaCorrenteCommand request, CancellationToken cancellationToken)
        {
            var contaCorrente = new Domain.Entities.ContaCorrente(request.Numero, request.Nome, request.Ativo);

            await _contaCorrenteRepository.AddContaCorrente(
                new AddContaCorrenteRequest
                {
                    IdContaCorrente = contaCorrente.IdContaCorrente,
                    Numero = contaCorrente.Numero,
                    Nome = contaCorrente.Nome,
                    Ativo = contaCorrente.Ativo,
                });

            await _unitOfWork.CommitAsync();

            return new AddContaCorrenteResponse
            {
                IdContaCorrente = contaCorrente.IdContaCorrente,
                Numero = contaCorrente.Numero,
                Nome = contaCorrente.Nome,
                Ativo = contaCorrente.Ativo,
            };
        }
    }
}
