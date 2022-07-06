using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Social.Network.Domain.Entities;
using Social.Network.Message.Commands;
using Social.Network.Message.Dtos;
using Social.Network.Repository;
using Social.Network.SeedWorks;
using Social.Network.SeedWorks.Helpers;
using Social.Network.SeedWorks.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Social.Network.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ApiControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        public readonly ITokenGenerator _tokenGenerator;


        public UserController(IUnitOfWork unitOfWork, ITokenGenerator tokenGenerator, UserManager<User> userManager)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _tokenGenerator = tokenGenerator;
        }

        [HttpPost("loign")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginUserCommand command)
        {
            var user = await _unitOfWork.UserRepository.FirstOrDefaultAsync(c => c.Email.ToLower() == command.Email.ToLower());
            if (user is null)
                return NotFound("Invalid user name or password.");

            if (!await _userManager.CheckPasswordAsync(user, command.Password))
                return BadRequest("Invalid user name or password.");

            var userRoles = await _userManager.GetRolesAsync(user);
            var token = _tokenGenerator.TokenGeneration(user, new List<IdentityRole>());

            return OkResult("You logged in successfully.", token);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterCommand command)
        {
            command.Validate();

            try
            {
                if (await _unitOfWork.UserRepository.AnyAsync(c => c.UserName.ToLower() == command.Name.ToLower()))
                    return BadRequest("The entered name has already been registered.");

                if (await _unitOfWork.UserRepository.AnyAsync(c => c.Email.ToLower() == command.Email.ToLower()))
                    return BadRequest("The entered email has already been registered.");

                await _unitOfWork.UserRepository.CreateAsync(command);
                await _unitOfWork.CommitAsync();
                return OkResult("You have been registered successfully.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest("An error occurred while registering user.");
            }
        }

        [HttpPost("UpdateUserInfo")]
        public async Task<IActionResult> UpdateUserInfo(UserUpdateCommand command)
        {
            try
            {
                await _unitOfWork.UserRepository.UpdateUserAsync(command, UserId);
                await _unitOfWork.CommitAsync();

                return OkResult("Your information is updated successfully.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest("An error occurred while updating user information.");
            }
        }

        [HttpGet("GetUserDetail")]
        public async Task<IActionResult> GetUserDetail(string userId = null)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.FirstOrDefaultAsync(c => c.Id == (userId ?? UserId.ToString()));
                var currentUser = await _unitOfWork.UserRepository.FirstOrDefaultAsync(c => c.Id == UserId.ToString());
                if (user is null)
                    return NotFound("User is not found.");

                return OkResult("User is found", new UserDto()
                {
                    Address = user.Address,
                    Avatar = user.Avatar,
                    Country = user.Country,
                    Description = user.Description,
                    FirstName = currentUser.FirstName,
                    LastName = currentUser.LastName,
                    Postcode = user.PostCode,
                    TownOrCity = user.TownOrCity,
                    Email = user.Email,
                    Username = currentUser.UserName,
                    PhoneNumber = user.PhoneNumber
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest("An error occurred while gettig user information.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUser(Guid? id)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.
                    GetFirstWithIncludeAsync(c => id != null ? c.Id == id.Value.ToString() : c.Id == UserId.ToString(), c => c.Posts, t => t.Friends);

                if (user is null)
                    return NotFound("User is not found.");

                return OkResult("User is found", new UserPageDto()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Description = user.Description,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    Avatar = user.Avatar,
                    Friends = user.Friends is null || !user.Friends.Any() ? new List<UserDto>() : user.Friends.Select(s => new UserDto()
                    {
                        Id = s.Id,
                        FirstName = s.FirstName,
                        LastName = s.LastName,
                        Username = s.UserName,
                        Avatar = s.Avatar
                    }).ToList(),
                    LastName = user.LastName,
                    Posts = user.Posts is null || !user.Posts.Any() ? new List<PostDto>() : user.Posts.Select(s => new PostDto()
                    {
                        Id = s.Id,
                        Content = s.Content,
                        Image = s.Image,
                        Time = s.CreationDate.CalculateTime(),
                        PostOwnerId = user.Id,
                        PostOwnerAvatar = user.Avatar
                    }).ToList()
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest("An error occurred while gettig user information.");
            }
        }

        [HttpGet("CheckUser/{id}")]
        public async Task<IActionResult> CheckUser(Guid id)
        {
            try
            {
                if (await _unitOfWork.UserRepository.AnyAsync(v => v.Id == id.ToString()) && id != UserId)
                    return OkResult("User is found.");
                else
                    return NotFound("User is not found!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest("An error occurred while gettig user information.");
            }
        }

        [HttpPost("AddFriend/{id}")]
        public async Task<IActionResult> AddFriend(Guid id)
        {
            try
            {
                await _unitOfWork.UserRepository.AddFriendAsync(UserId, id);
                await _unitOfWork.CommitAsync();

                return OkResult("You have new friend now!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest("An error occurred while adding friend.");
            }
        }

        [HttpDelete("RemoveFriend/{id}")]
        public async Task<IActionResult> RemoveFriend(Guid id)
        {
            try
            {
                await _unitOfWork.UserRepository.RemoveFriendAsync(UserId, id);
                await _unitOfWork.CommitAsync();

                return OkResult("You have removed your friend!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest("An error occurred while removing friend.");
            }
        }

        [HttpGet("CheckFriend/{id}")]
        public async Task<IActionResult> CkeckFriend(Guid id)
        {
            try
            {
                var currentUser = await _unitOfWork.UserRepository.GetFirstWithIncludeAsync(c => c.Id == UserId.ToString(), t => t.Friends);
                if (currentUser.Friends.Any(c => c.Id == id.ToString()))
                    return OkResult("That's your friend");
                else
                    return NotFound("That's not your friend!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest("An error occurred while getting friend info!");
            }
        }
    }
}
