using MediatR;
using Questao5.Infrastructure.Database.CommandStore.Responses.ContaCorrente;

namespace Questao5.Application.ContaCorrente.Commands
{
    public class UpdateContaCorrenteCommand : IRequest<UpdateContaCorrenteResponse>
    {
        public string IdContaCorrente { get; set; }
        public int Numero { get; set; }
        public string Nome { get; set; }
        public bool Ativo { get; set; }
    }
}
