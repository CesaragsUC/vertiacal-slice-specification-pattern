using FluentValidation;

namespace Application.Categories.Create;

public class CreateCategoryCommandValidation : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidation()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .MinimumLength(2).WithMessage("O nome da categoria deve ter no minimo 2 caracteres.")
            .MaximumLength(100).WithMessage("O nome categoria pode ter no máximo 100 caracteres.");
    }
}
