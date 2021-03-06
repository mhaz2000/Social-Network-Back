using Microsoft.AspNetCore.Mvc;
using Social.Network.Message.Commands;
using Social.Network.Message.Dtos;
using Social.Network.Repository;
using Social.Network.SeedWorks;
using Social.Network.SeedWorks.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Social.Network.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ApiControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public PostController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(CreatePostCommand command)
        {
            command.Validate();
            try
            {
                var postId = await _unitOfWork.PostRepository.CreatePost(command, UserId);
                await _unitOfWork.CommitAsync();
                return OkResult("The post is created successfully", postId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest("Something went wrong while creating new post!");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(Guid id)
        {
            try
            {
                var currentUser = await _unitOfWork.UserRepository.FirstOrDefaultAsync(c => c.Id == UserId.ToString());
                var post = await _unitOfWork.PostRepository.GetFirstWithIncludeAsync(c => c.Id == id, t => t.Comments);

                var postOwner = await _unitOfWork.UserRepository.FirstOrDefaultAsync(c => c.Id == post.PostOwnerId.ToString());

                return OkResult("Post is found", new PostDto()
                {
                    Content = post.Content,
                    Id = post.Id,
                    Image = post.Image,
                    Time = post.CreationDate.CalculateTime(),
                    PostOwnerId = post.PostOwnerId.ToString(),
                    PostOwnerAvatar = postOwner.Avatar,
                    CurrentUserAvatar = currentUser.Avatar,
                    PostOwnerFirstName = postOwner.FirstName,
                    PostOwnerLastName = postOwner.LastName,
                    PostOwnerUsername = postOwner.UserName,
                    Comments = post.Comments is null || !post.Comments.Any() ? new List<CommentDto>() : post.Comments.Select(s => new CommentDto()
                    {
                        Content = s.Content,
                        CommentOwnerId = s.CommentOwnerId
                    }).ToList()
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest("Something went wrong while getting the post!");
            }
        }


        [HttpGet("GetUserPosts")]
        public async Task<IActionResult> GetUserPosts(int count, int skip)
        {
            try
            {
                var posts = (await _unitOfWork.PostRepository.FindAsync(c => c.PostOwnerId == UserId)).Skip(skip).Take(count);
                return OkResult("User posts", posts.Select(s => new PostDto()
                {
                    Content = s.Content,
                    Image = s.Image,
                    PostOwnerId = s.PostOwnerId.ToString()
                }));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest("Something went wrong while getting posts!");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var post = await _unitOfWork.PostRepository.GetByIdAsync(id);

            if (post is null)
                return NotFound("Post can't be found!");
            try
            {
                await _unitOfWork.PostRepository.DeletePost(id);
                await _unitOfWork.CommitAsync();

                DeleteFile(post.Image);
                return OkResult("Post was deleted successfully");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest("Something went wrong while deleting the post!");
            }

        }

        [HttpGet("GetAllPosts")]
        public async Task<IActionResult> GetPosts()
        {
            try
            {
                var posts = await _unitOfWork.PostRepository.GetListWithIncludeAsync("Comments");
                List<PostListDto> postsDto = new List<PostListDto>();

                foreach (var post in posts)
                {
                    var postOwner = await _unitOfWork.UserRepository.FirstOrDefaultAsync(c => c.Id == post.PostOwnerId.ToString());
                    postsDto.Add(new PostListDto()
                    {
                        CommentsCount = post.Comments.Count(),
                        Image = post.Image,
                        Content = post.Content,
                        Time = post.CreationDate.CalculateTime(),
                        Id = post.Id,
                        PostOwnerId = post.PostOwnerId,
                        PostOwnerAvatar = postOwner.Avatar,
                        FirstName = postOwner.FirstName,
                        LastName = postOwner.LastName,
                        Username  = postOwner.UserName
                    });
                }

                return OkResult("Get all posts", postsDto);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest("Something went wrong while getting posts!");
            }
        }

        private void DeleteFile(Guid id)
        {
            var teat = @$"{Directory.GetCurrentDirectory()}\FileStorage\{id}.png";
            if (System.IO.File.Exists(@$"{Directory.GetCurrentDirectory()}\FileStorage\{ id}.png"))
            {
                System.IO.File.Delete(@$"{Directory.GetCurrentDirectory()}\FileStorage\{id}.png");
            }
        }
    }
}
