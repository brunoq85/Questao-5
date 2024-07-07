using MediatR;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database.CommandStore.Requests.ContaCorrente;
using Questao5.Infrastructure.Database.CommandStore.Responses.ContaCorrente;

namespace Questao5.Application.ContaCorrente.Commands
{
    public class AddContaCorrenteCommand : IRequest<AddContaCorrenteResponse>
    {
        public string IdContaCorrente { get; set; }
        public int Numero { get; set; }
        public string Nome { get; set; }
        public bool Ativo { get; set; }        
    }
}
