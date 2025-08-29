using FluentValidation;

namespace Application.Product.Create;

public class CreateProductComandValidation : AbstractValidator<CreateProductCommand>
{
    public CreateProductComandValidation()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("The name is required.")
            .MinimumLength(2).WithMessage("The name must be at least 2 characters long.")
            .MaximumLength(100).WithMessage("The name must be at most 100 characters.");
        RuleFor(c => c.Price)
            .GreaterThan(0).WithMessage("The price must be greater than zero.");
        RuleFor(c => c.CategoryId)
            .NotEmpty().WithMessage("The category ID is required.");
    }
}