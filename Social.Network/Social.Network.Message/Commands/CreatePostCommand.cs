using Social.Network.Message.Extensions;
using Social.Network.Message.Validators;
using System;

namespace Social.Network.Message.Commands
{
    public class CreatePostCommand
    {
        public string Content { get; set; }
        public string Image { get; set; }

        public void Validate()
        {
            new CreatePostCommandValidator().Validate(this).RaiseExceptionIfRequired();

        }
    }
}
