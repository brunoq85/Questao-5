using Questao5.Domain.Enumerators;
using Questao5.Domain.Validation;

namespace Questao5.Domain.Entities;

public class Movimento
{
    public string IdMovimento { get; private set; }
    public string IdContaCorrente { get; private set; }
    public DateTime DataMovimento { get; private set; }
    public char TipoMovimento { get; private set; }
    public decimal Valor { get; private set; }
    public ContaCorrente? ContaCorrente { get; private set; }

    public Movimento() { }

    public Movimento(string idMovimento, string idContaCorrente, DateTime dataMovimento, char tipoMovimento, decimal valor, ContaCorrente? contaCorrente)
    {
        ValidateDomain(idMovimento, idContaCorrente, dataMovimento, tipoMovimento, valor);
        contaCorrente.ValidarContaAtiva();

        IdMovimento = idMovimento;
        IdContaCorrente = idContaCorrente;
        DataMovimento = dataMovimento;
        TipoMovimento = tipoMovimento;
        Valor = valor;
        ContaCorrente = contaCorrente;
    }

    public void Update(string idMovimento, string idContaCorrente, DateTime dataMovimento, char tipoMovimento, decimal valor)
    {
        ValidateDomain(idMovimento, idContaCorrente, dataMovimento, tipoMovimento, valor);
        ContaCorrente.ValidarContaAtiva();
    }

    public void ValidarMovimento()
    {
        if (string.IsNullOrWhiteSpace(IdContaCorrente))
        {
            throw new ArgumentException("A conta corrente é inválida.");
        }

        ContaCorrente.ValidarContaAtiva();

        if (Valor <= 0)
        {
            throw new ArgumentException("O valor do movimento deve ser positivo.");
        }

        if (TipoMovimento != (char)ETipoMovimento.CREDITO && TipoMovimento != (char)ETipoMovimento.DEBITO)
        {
            throw new ArgumentException("Tipo de movimento inválido. Use 'C' para crédito ou 'D' para débito.");
        }
    }

    private void ValidateDomain(string idMovimento, string idContaCorrente, DateTime dataMovimento, char tipoMovimento, decimal valor)
    {
        // Validação campo: idMovimento
        DomainValidation.When(string.IsNullOrEmpty(idMovimento), "O identificador do movimento deve ser informado.");
        DomainValidation.When(idMovimento.Length > 37, "O identificador do movimento deve ter até 37 caracteres");

        // Validação campo: idContaCorrente
        DomainValidation.When(string.IsNullOrEmpty(idContaCorrente), "O identificador da conta corrente deve ser informado.");
        DomainValidation.When(idContaCorrente.Length > 37, "O identificador da conta corrente deve ter até 37 caracteres");

        // Validação campo: dataMovimento
        DomainValidation.When(dataMovimento < DateTime.Now, "Data inválida");

        // Validação campo: valor
        DomainValidation.When(valor <= 0, "O valor do movimento deve ser positivo.");
    }
}
