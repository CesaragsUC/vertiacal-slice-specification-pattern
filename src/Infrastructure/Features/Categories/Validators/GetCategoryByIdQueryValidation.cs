using Application.Features.Categories.EndPoints;
using FluentValidation;

namespace Application.Features.Categories.Validators;

public class GetCategoryByIdQueryValidation : AbstractValidator<GetCategoryByIdRequest>
{
    public GetCategoryByIdQueryValidation()
    {

        RuleFor(c => c.Id)
            .NotEmpty().WithMessage("O ID é obrigatório.")
            .Must(id => id != Guid.Empty).WithMessage("O ID não pode ser um GUID vazio.");
    }
}
