using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Social.Network.Domain.Entities;
using Social.Network.Message.Commands;
using Social.Network.Repository;
using Social.Network.SeedWorks;
using Social.Network.SeedWorks.Interfaces;
using System.Collections.Generic;
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
            catch (System.Exception e)
            {
                return BadRequest("An error occurred while registering user.");
            }
        }
    }
}
