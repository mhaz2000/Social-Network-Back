using Social.Network.Message.Extensions;
using Social.Network.Message.Validators;

namespace Social.Network.Message.Commands
{
    public class RegisterCommand
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public void Validate()
        {
            new RegisterCommandValidator().Validate(this).RaiseExceptionIfRequired();

        }
    }
}
