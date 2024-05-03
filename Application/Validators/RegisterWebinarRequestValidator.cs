using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Application.Requests;
using FluentValidation;

namespace Application.Validators;

[ExcludeFromCodeCoverage]
public class RegisterWebinarRequestValidator : AbstractValidator<RegisterWebinarRequest>
{
    private static readonly Regex PhoneRegex = new(@"^07[0-8]{1}[0-9]{7}$");
    public RegisterWebinarRequestValidator()
    {
        RuleForPersonEmail();
        RulesForName();
        RulesForPhoneNumber();
    }

    private void RuleForPersonEmail()
    {
        RuleFor(x => x.Person.Email)
            .NotNull()
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Person email validation was violated");
    }
    
    private void RulesForName()
    {
        RuleFor(x => x.Person.Name)
            .NotNull()
            .NotEmpty()
            .Must(IsValidName)
            .WithMessage("Name validation was violated");
    }
    
    private void RulesForPhoneNumber()
    {
        RuleFor(x => x.Person.PhoneNumber)
            .Must(IsValidPhoneNumber)
            .WithMessage("Phone number validation was violated");
    }

    private bool IsValidName(string? name)
    {
        return !(name?.Any(char.IsDigit) == true ||
                 name?.Any(char.IsSymbol) == true);
    }
    
    private bool IsValidPhoneNumber(string? phoneNumber)
    {
        if (string.IsNullOrEmpty(phoneNumber))
        {
            return true;
        }
        
        return PhoneRegex.Match(phoneNumber).Success;
    }
}