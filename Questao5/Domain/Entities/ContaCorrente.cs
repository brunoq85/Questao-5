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

        public ContaCorrente(int numero, string nome, bool ativo) 
        {
            ValidateDomain(numero, nome, ativo);

            IdContaCorrente = Guid.NewGuid().ToString();
            Numero = numero;
            Nome = nome;
            Ativo = ativo;
        }

        public ContaCorrente(string idContaCorrente, int numero, string nome, bool ativo)
        {
            // Validação campo: idContaCorrente
            DomainValidation.When(string.IsNullOrEmpty(idContaCorrente), "O identificador da conta corrente deve ser informado.");
            DomainValidation.When(idContaCorrente.Length > 37, "O identificador da conta corrente deve ter até 37 caracteres");

            ValidateDomain(numero, nome, ativo);

            IdContaCorrente = idContaCorrente;
            Numero = numero;
            Nome = nome;
            Ativo = ativo;
        }

        private void ValidateDomain(int numero, string nome, bool ativo)
        {
            // Validação campo: numero
            DomainValidation.When(numero == 0 || numero == null, "O número da conta corrente deve ser informado.");
            DomainValidation.When(numero.ToString().Count() > 10, "O número da conta corrente deve ter até 10 números");
            DomainValidation.When(numero <= 0, "O número da conta corrente deve ser positivo.");

            // Validação campo: nome
            DomainValidation.When(string.IsNullOrEmpty(nome), "O nome do titular da conta corrente deve ser informado.");
            DomainValidation.When(nome.Length > 100, "O nome do titular da conta corrente deve ter até 100 caracteres.");

            // Validação campo: ativo
            DomainValidation.When(!ativo, "A conta corrente está inativa.");
        }
    }
}
