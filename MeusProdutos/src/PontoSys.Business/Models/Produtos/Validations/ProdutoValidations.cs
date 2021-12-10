using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PontoSys.Business.Models.Produtos.Validations
{
    public class ProdutoValidations : AbstractValidator<Produto>
    {
        public ProdutoValidations()
        {
            RuleFor(c => c.Nome)
                .NotEmpty().WithMessage("O Campo {PropertyName} precisa ser fornecido")
                .Length(2, 200).WithMessage("O Campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(c => c.Descricao)
                .NotEmpty().WithMessage("O Campo {PropertyName} precisa ser fornecido")
                .Length(2, 1000).WithMessage("O Campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(c => c.Valor)
                .GreaterThan(0).WithMessage("O Campo {PropertyName} precisa ser maior que {ComparisonValue}");
        }
    }
}
