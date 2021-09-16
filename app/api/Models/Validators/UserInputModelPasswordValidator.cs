using System.Text.RegularExpressions;
using api.Models.Inputs;
using FluentValidation;

namespace api.Models.Validators
{
    public class UserInputModelPasswordValidator : AbstractValidator<string>
    {
        public UserInputModelPasswordValidator()
        {
            RuleFor(password => password)
                .NotEmpty().WithMessage("password should not be left empty")
                .NotNull().WithMessage("password input is missing")
                .Custom((password, context) => {
                    int total = 0;
                    total += Regex.Match(password, "[\\W]+").Captures.Count;
                    total += Regex.Match(password, "[A-Za-z]+").Captures.Count;
                    total += Regex.Match(password, "[\\d]+").Captures.Count;
                    if(total is not 3)
                    {
                        context.AddFailure("password must contain alphanumeric and symbol chars");
                    }
                })
                .MinimumLength(8).WithMessage("password must have min. length of 8");
        }
    }
}