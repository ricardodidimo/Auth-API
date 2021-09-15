using System.Text.RegularExpressions;
using api.Models.Inputs;
using FluentValidation;

namespace api.Models.Validators
{
    public class UserInputModelValidator : AbstractValidator<UserInputModel>
    {
        public UserInputModelValidator()
        {
            RuleFor(user => user.username)
                .NotEmpty().WithMessage("username should not be left empty")
                .NotNull().WithMessage("username input is missing")
                .Custom((username, context) => {
                   MatchCollection searchBackspaces = Regex.Matches(username, "\\s");
                    if(searchBackspaces.Count > 0)
                    {
                        context.AddFailure("username must not contain blank spaces");
                    }
                })
                .Length(2,25).WithMessage("username must have min. length of 2 and a max of 25 chars");
            
            RuleFor(user => user.password)
                .NotEmpty().WithMessage("password should not be left empty")
                .NotNull().WithMessage("password input is missing")
                .MinimumLength(6).WithMessage("password must have min. length of 6");
        }
    }
}