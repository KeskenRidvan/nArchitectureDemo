using FluentValidation;

namespace Application.Features.Brands.Commands.Create;
public class CreateBrandCommandValidator : AbstractValidator<CreateBrandCommand>
{
    public CreateBrandCommandValidator()
    {
        RuleFor(b => b.Name)
            .NotEmpty().WithMessage("The name field cannot be empty!")
            .MinimumLength(2).WithMessage("The name field cannot be empty, it must be at least 2 characters!");
    }
}
