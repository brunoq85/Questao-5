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

    public Movimento(string idContaCorrente, char tipoMovimento, decimal valor)
    {
        ValidateDomain(idContaCorrente, tipoMovimento, valor);

        IdMovimento = Guid.NewGuid().ToString();
        IdContaCorrente = idContaCorrente;
        DataMovimento = DateTime.Now;
        TipoMovimento = tipoMovimento;
        Valor = valor;
    }

    public Movimento(string idMovimento, string idContaCorrente, DateTime dataMovimento, char tipoMovimento, decimal valor)
    {
        // Validação campo: idMovimento
        DomainValidation.When(string.IsNullOrEmpty(idMovimento), "O identificador do movimento deve ser informado.");
        DomainValidation.When(idMovimento.Length > 37, "O identificador do movimento deve ter até 37 caracteres");

        // Validação campo: dataMovimento
        DomainValidation.When(dataMovimento < DateTime.Now, "Data inválida");

        ValidateDomain(idContaCorrente, tipoMovimento, valor);

        IdMovimento = idMovimento;
        IdContaCorrente = idContaCorrente;
        DataMovimento = DateTime.Now ;
        TipoMovimento = tipoMovimento;
        Valor = valor;
    }  

    private void ValidateDomain(string idContaCorrente, char tipoMovimento, decimal valor)
    {
        // Validação campo: idContaCorrente
        DomainValidation.When(string.IsNullOrEmpty(idContaCorrente), "O identificador da conta corrente deve ser informado.");
        DomainValidation.When(idContaCorrente.Length > 37, "O identificador da conta corrente deve ter até 37 caracteres");

        // Validação campo: valor
        DomainValidation.When(valor <= 0, "O valor do movimento deve ser positivo.");

        // Validação campo: tipoMovimento
        DomainValidation.When(tipoMovimento != (char)ETipoMovimento.CREDITO && tipoMovimento != (char)ETipoMovimento.DEBITO, "Tipo de movimento inválido. Use 'C' para crédito ou 'D' para débito.");
    }
}
