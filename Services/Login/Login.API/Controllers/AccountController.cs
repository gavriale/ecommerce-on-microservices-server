using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AddUserBalanceCommand;
using Login.API.Data;
using Login.API.DTOs;
using Login.API.Entities;
using Login.API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NServiceBus;

namespace Login.API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        private readonly ILogger<AccountController> _logger;
        private readonly IMessageSession _messageSession;
        public AccountController(DataContext context, ITokenService tokenService,ILogger<AccountController> logger,
            IMessageSession messageSession)
        {
            _tokenService = tokenService;
            _context = context;
            _logger = logger;
            _messageSession = messageSession;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Username)) return BadRequest("Username is taken");

            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                UserName = registerDto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            _logger.LogInformation("------------Creating User!-------------------");

            var userCreated = await _context.Users
                .SingleOrDefaultAsync(x => x.UserName == user.UserName);

            _logger.LogInformation("------------Sending Message AddUserBalance!-------------------");

            await _messageSession.Send(
            new AddUserBalance
            {
                UserId = userCreated.Id,
                Balance = 500
            });

            return new UserDto
            {
                UserId = user.Id,
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _context.Users
                .SingleOrDefaultAsync(x => x.UserName == loginDto.Username);

            if (user == null) return Unauthorized("Invalid username");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
            }

            return new UserDto
            {
                UserId = user.Id,
                Token = _tokenService.CreateToken(user)
            };
        }

        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}
