using FluentValidation;

namespace Application.Product.Delete;

public class DeleteProductComandValidation : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductComandValidation()
    {
        RuleFor(c => c.Id)
            .NotEmpty().WithMessage("O Id is required.");

    }
}