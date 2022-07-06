using FluentValidation;
using Social.Network.Message.Commands;

namespace Social.Network.Message.Validators
{
    class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
    {
        public CreatePostCommandValidator()
        {
            RuleFor(c => c.Image).NotNull().NotEmpty().WithMessage("Please add an image for post.");
        }
    }
}
