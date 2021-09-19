using api.Models.Inputs;
using FluentValidation;
using FluentValidation.Results;

namespace api.Models.Validators
{
    public class UserInputModelValidator : AbstractValidator<UserInputModel>
    {
        public UserInputModelValidator()
        {
            RuleFor(user => user.username)
                .Custom((username, context) => {
                    var validator = new UserInputModelUsernameValidator();
                    ValidationResult result = validator.Validate(username);

                    if(result.IsValid is false)
                    {
                        foreach (ValidationFailure item in result.Errors)
                        {
                            context.AddFailure(item);
                        }
                    }
                });

            RuleFor(user => user.password)
                .Custom((password, context) => {
                    var validator = new UserInputModelPasswordValidator();
                    ValidationResult result = validator.Validate(password);

                    if(result.IsValid is false)
                    {
                        foreach (ValidationFailure item in result.Errors)
                        {
                            context.AddFailure(item);
                        }
                    }
                });

        }
    }
}