using Microsoft.AspNetCore.Mvc;
using Social.Network.Message.Commands;
using Social.Network.Message.Dtos;
using Social.Network.Repository;
using Social.Network.SeedWorks;
using Social.Network.SeedWorks.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Social.Network.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ApiControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public CommentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> PostComment(CreateCommentCommand command)
        {
            try
            {
                var commentId = await _unitOfWork.CommentRepository.CreateComment(command, UserId);
                await _unitOfWork.CommitAsync();
                return OkResult("Comment was added successfully.", commentId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest("An error occurred while gettig user information.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostComment(Guid id)
        {
            try
            {
                var comments = (await _unitOfWork.PostRepository.GetFirstWithIncludeAsync(c => c.Id == id, t => t.Comments)).Comments;
                var users = await _unitOfWork.UserRepository.GetAllAsync();

                return OkResult("Post comments were found.", comments.Select(s => new CommentDto()
                {
                    CommentOwnerId = s.CommentOwnerId,
                    Content = s.Content,
                    Time = s.CreationDate.CalculateTime(),
                    UserName = _unitOfWork.UserRepository.FirstOrDefault(c => c.Id == s.CommentOwnerId.ToString()).UserName,
                    FirstName = _unitOfWork.UserRepository.FirstOrDefault(c => c.Id == s.CommentOwnerId.ToString()).FirstName,
                    LastName = _unitOfWork.UserRepository.FirstOrDefault(c => c.Id == s.CommentOwnerId.ToString()).LastName,
                    Avatar = _unitOfWork.UserRepository.FirstOrDefault(c => c.Id == s.CommentOwnerId.ToString()).Avatar
                }));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest("An error occurred while gettig post comment.");
            }
        }

        //[HttpPut("{id}")]
        //public async Task<IActionResult> LikeComment(Guid id)
        //{
        //    try
        //    {
        //        var comment = await _unitOfWork.CommentRepository.GetFirstWithIncludeAsync(c => c.Id == id, c => c.Likes);

        //        if (comment.Likes.Any(c => c.LikedById == UserId))
        //            comment.Likes.Remove(comment.Likes.FirstOrDefault(c => c.LikedById == UserId));
        //        else
        //            comment.Likes.Add(new Domain.Entities.Like() { LikedById = UserId });

        //        await _unitOfWork.CommitAsync();
        //        return OkResult("You liked this post");
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //        return BadRequest("An error occurred while liking comment.");
        //    }
        //}

    }
}
