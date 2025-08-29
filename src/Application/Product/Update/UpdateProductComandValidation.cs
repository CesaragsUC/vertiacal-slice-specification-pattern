using FluentValidation;

namespace Application.Product.Update;

public class UpdateProductComandValidation : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductComandValidation()
    {
        RuleFor(c => c.Id)
            .NotEmpty().WithMessage("The product ID is required.");
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("The name is required.")
            .MinimumLength(2).WithMessage("The name must be at most 100 characters.")
            .MaximumLength(100).WithMessage("The name must be at most 100 characters.");
        RuleFor(c => c.Price)
            .GreaterThan(0).WithMessage("The price must be greater than zero.");
        RuleFor(c => c.CategoryId)
            .NotEmpty().WithMessage("The category ID is required.");
    }
}