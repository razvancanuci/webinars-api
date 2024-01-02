﻿using Application.Requests;
using FluentValidation;

namespace Application.Validators;

public class NewWebinarRequestValidator : AbstractValidator<NewWebinarRequest>
{
    public NewWebinarRequestValidator()
    {
        RuleForTitle();
        RuleForDescription();
        RuleForHost();
        RuleForScheduledDate();
    }

    private void RuleForTitle()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .NotNull()
            .MinimumLength(4)
            .WithMessage("Title validation was violated");
    }

    private void RuleForDescription()
    {
        RuleFor(x => x.Description)
            .NotNull()
            .NotEmpty()
            .MinimumLength(15)
            .WithMessage("Description validation was violated");

    }

    private void RuleForHost()
    {
        RuleFor(x => x.Host)
            .NotNull()
            .NotEmpty()
            .Length(2, 15)
            .WithMessage("Host validation was violated");
    }

    private void RuleForScheduledDate()
    {
        RuleFor(x => x.DateScheduled)
            .NotNull()
            .NotEmpty().
            GreaterThan(DateTime.UtcNow.AddDays(3));
    }
}