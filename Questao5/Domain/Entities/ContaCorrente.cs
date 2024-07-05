using NSubstitute;
using Questao5.Domain.Validation;

namespace Questao5.Domain.Entities
{
    public class ContaCorrente
    {
        public string IdContaCorrente { get; private set; }
        public int Numero { get; private set; }
        public string Nome { get; private set; }
        public bool Ativo { get; private set; }

        public ContaCorrente() { }

        public ContaCorrente(string idContaCorrente, int numero, string nome, bool ativo)
        {
            ValidateDomain(idContaCorrente, numero, nome, ativo);
        }

        public void Update(string idContaCorrente, int numero, string nome, bool ativo)
        {
            ValidateDomain(idContaCorrente, numero, nome, ativo);
        }

        public void ValidarContaAtiva()
        {
            if (!Ativo)
            {
                throw new InvalidOperationException("A conta corrente está inativa.");
            }
        }

        private void ValidateDomain(string idContaCorrente, int numero, string nome, bool ativo)
        {
            // Validação campo: idContaCorrente
            DomainValidation.When(string.IsNullOrEmpty(idContaCorrente), "O identificador da conta corrente deve ser informado.");
            DomainValidation.When(idContaCorrente.Length > 37, "O identificador da conta corrente deve ter até 37 caracteres");

            // Validação campo: numero
            DomainValidation.When(numero == 0 || numero == null, "O número da conta corrente deve ser informado.");
            DomainValidation.When(idContaCorrente.Length > 10, "O número da conta corrente deve ter até 10 números");

            // Validação campo: nome
            DomainValidation.When(string.IsNullOrEmpty(idContaCorrente), "O nome do titular da conta corrente deve ser informado.");
            DomainValidation.When(idContaCorrente.Length > 100, "O nome do titular da conta corrente deve ter até 100 caracteres");

            // Validação campo: ativo
            DomainValidation.When(!ativo, "Deve definir uma atividade");
        }
    }
}
