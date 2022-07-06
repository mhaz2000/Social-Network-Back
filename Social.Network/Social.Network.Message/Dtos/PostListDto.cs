using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.Network.Message.Dtos
{
    public class PostListDto
    {
        public int CommentsCount { get; set; }
        public Guid Image { get; set; }
        public string Content { get; set; }
        public string Time { get; set; }
        public Guid Id { get; set; }
        public Guid PostOwnerId { get; set; }
        public string PostOwnerAvatar { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Username { get; set; }
    }
}
