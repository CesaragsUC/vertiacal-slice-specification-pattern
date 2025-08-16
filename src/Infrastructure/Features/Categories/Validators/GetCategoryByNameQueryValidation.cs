using Application.Features.Categories.EndPoints;
using FluentValidation;

namespace Application.Features.Categories.Validators;

public class GetCategoryByNameQueryValidation : AbstractValidator<GetCategoryByNameRequest>
{
    public GetCategoryByNameQueryValidation()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .MinimumLength(2).WithMessage("O nome deve ter no mínimo 2 caracteres.")
            .MaximumLength(100).WithMessage("O nome deve ter no máximo 100 caracteres.");
    }
}
