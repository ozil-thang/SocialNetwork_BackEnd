using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Api.Models.Auth;
using Api.Utils;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using Microsoft.Extensions.Configuration;
using Api.Models.User;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    [Authorize]
    public class AuthController : MyControllerBase
    {
        private readonly SocialNetworkContext _context;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly RedisDatabaseProvider _redisDatabaseProvider;

        public AuthController(SocialNetworkContext context, IConfiguration config, IMapper mapper, RedisDatabaseProvider redisDatabaseProvider)
        {
            _context = context;
            _config = config;
            _mapper = mapper;
            _redisDatabaseProvider = redisDatabaseProvider;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterDto registerDto)
        {

            if (await _context.Users.Where(x => x.Email == registerDto.Email).AnyAsync())
                return BadRequest(new Error("User already exits"));

            byte[] passwordHash, passwordSalt;
            PasswordUtil.CreatePasswordHash(registerDto.Password, out passwordHash, out passwordSalt);

            var newUser = new User();
            newUser.Email = registerDto.Email;
            newUser.PasswordHash = passwordHash;
            newUser.PasswordSalt = passwordSalt;

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            var tokenString = GetToken(newUser);
            return Ok(new { token = tokenString });
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginDto loginDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == loginDto.Email);
            if (user == null || !PasswordUtil.VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt))
                return NotFound();

            var tokenString = GetToken(user);

            return Ok(new { token = tokenString });

        }



        [HttpGet]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == UserId);
            var userDto = _mapper.Map<UserDto>(user);

            var redisDb = _redisDatabaseProvider.GetDatabase();

            redisDb.ListRightPush("onlineuser", user.Email);

            return Ok(userDto);
        }

        private string GetToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config.GetSection("AppSettings:SecretKey").Value);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.Email)
                }),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}
