using System;

namespace Social.Network.Message.Commands
{
    public class CreateCommentCommand
    {
        public string Content { get; set; }
        public Guid PostId { get; set; }
    }
}
