using System.Diagnostics.CodeAnalysis;
using Application.Requests;
using FluentValidation;

namespace Application.Validators;

[ExcludeFromCodeCoverage]
public class AvailableWebinarsRequestValidator : AbstractValidator<AvailableWebinarsRequest>
{
    public AvailableWebinarsRequestValidator()
    {
        RuleForPage();
    }

    private void RuleForPage()
    {
        RuleFor(x => x.Page)
            .NotNull()
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("Page validation was violated");
    }
}