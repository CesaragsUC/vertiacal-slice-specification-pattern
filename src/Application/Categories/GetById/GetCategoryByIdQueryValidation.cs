using FluentValidation;

namespace Application.Categories.GetById;

public class GetCategoryByIdQueryValidation : AbstractValidator<GetCategoryByIdQuery>
{
    public GetCategoryByIdQueryValidation()
    {

        RuleFor(c => c.Id)
            .NotEmpty().WithMessage("O ID é obrigatório.")
            .Must(id => id != Guid.Empty).WithMessage("O ID não pode ser um GUID vazio.");
    }
}
