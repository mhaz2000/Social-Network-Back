using System;
using System.Collections.Generic;

namespace Social.Network.Domain.Entities
{
    public class Post : BaseEntity
    {
        public Post() : base()
        {

        }
        public Guid PostOwnerId { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public Guid Image { get; set; }
        public string Content { get; set; }
    }
}
