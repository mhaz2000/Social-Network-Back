using System;

namespace Social.Network.Message.Dtos
{
    public class CommentDto
    {
        public Guid CommentOwnerId { get; set; }
        public string Content { get; set; }
    }
}