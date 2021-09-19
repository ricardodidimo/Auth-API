using System.Text.RegularExpressions;
using FluentValidation;

namespace api.Models.Validators
{
    public class UserInputModelUsernameValidator : AbstractValidator<string>
    {
        public UserInputModelUsernameValidator()
        {
            RuleFor(username => username)
                .NotEmpty().WithMessage("username should not be left empty")
                .NotNull().WithMessage("username input is missing")
                .Custom((username, context) => {
                    MatchCollection searchedBackspaces = Regex.Matches(username, "\\s");
                    if(searchedBackspaces.Count > 0)
                    {
                        context.AddFailure("username must not contain blank spaces");
                    }
                })
                .Length(2,25).WithMessage("username must have min. length of 2 and a max of 25 chars");
            
        }
    }
}