using System;

namespace Social.Network.Domain.Entities
{
    public class Comment : BaseEntity
    {
        public Guid CommentOwnerId { get; set; }
        public string Content { get; set; }
    }
}
