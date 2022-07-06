using System;
using System.Collections.Generic;

namespace Social.Network.Domain.Entities
{
    public class Comment : BaseEntity
    {
        public Comment() : base()
        {

        }
        public Guid CommentOwnerId { get; set; }
        public string Content { get; set; }
        //public ICollection<Like> Likes { get; set; }
    }
}
