using MediatR;
using Questao5.Infrastructure.Database.CommandStore.Responses.Movimento;

namespace Questao5.Application.Movimento.Commands
{
    public class AddMovimentoCommand : IRequest<AddMovimentoResponse>
    {
        public string IdContaCorrente { get; set; }
        public char TipoMovimento { get; set; }
        public decimal Valor { get; set; }
    }
}
