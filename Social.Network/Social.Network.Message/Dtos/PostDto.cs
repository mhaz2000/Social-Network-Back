using System;
using System.Collections.Generic;

namespace Social.Network.Message.Dtos
{
    public class PostDto
    {
        public Guid Id { get; set; }
        public string PostOwnerId { get; set; }
        public Guid Image { get; set; }
        public string Content { get; set; }
        public string Time { get; set; }
        public ICollection<CommentDto> Comments { get; set; }
        public string PostOwnerAvatar { get; set; }
        public string CurrentUserAvatar { get; set; }
        public string PostOwnerFirstName { get; set; }
        public string PostOwnerLastName { get; set; }
        public string PostOwnerUsername { get; set; }
    }
}
