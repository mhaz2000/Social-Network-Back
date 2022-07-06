using System.Collections.Generic;

namespace Social.Network.Message.Dtos
{
    public class UserPageDto
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string Avatar { get; set; }
        public ICollection<PostDto> Posts { get; set; }
        public ICollection<UserDto> Friends  { get; set; }
        public string Id { get; set; }
    }
}
