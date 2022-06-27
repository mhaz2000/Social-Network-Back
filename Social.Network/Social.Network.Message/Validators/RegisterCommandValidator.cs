using FluentValidation;
using Social.Network.Message.Commands;

namespace Social.Network.Message.Validators
{

    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email can not be empty.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name can not be empty.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password can not be empty.");

            RuleFor(x => x.Email).Must(c => c.IsValidEmail()).WithMessage("The format of email is not valid.");
            RuleFor(x => x.Password).MinimumLength(4).WithMessage("Password must be at least 4 characters long.");
        }
    }
}
