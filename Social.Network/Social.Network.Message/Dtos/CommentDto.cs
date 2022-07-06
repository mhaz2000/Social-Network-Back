using System;

namespace Social.Network.Message.Dtos
{
    public class CommentDto
    {
        public Guid CommentOwnerId { get; set; }
        public string Content { get; set; }
        public string Time { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Avatar { get; set; }
        public Guid Id { get; set; }
    }
}