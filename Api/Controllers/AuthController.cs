using System.Diagnostics;
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
using Microsoft.AspNetCore.SignalR;
using Api.Hubs;
using StackExchange.Redis;

namespace Api.Controllers
{
    [Authorize]
    public class AuthController : MyControllerBase
    {
        private readonly SocialNetworkContext _context;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly RedisDatabaseProvider _redisDatabaseProvider;

        private IHubContext<OnlineUserHub> _onlineUserHubContext;
        public AuthController(SocialNetworkContext context, IConfiguration config, IMapper mapper, RedisDatabaseProvider redisDatabaseProvider,
                                IHubContext<OnlineUserHub> onlineUserHubContext)
        {
            _context = context;
            _config = config;
            _mapper = mapper;
            _redisDatabaseProvider = redisDatabaseProvider;
            _onlineUserHubContext = onlineUserHubContext;
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
            return Ok(userDto);
        }

        [HttpGet("openTab")]
        public async Task<IActionResult> OnlineUser()
        {

            var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.UserId == UserId);

            var db = _redisDatabaseProvider.GetDatabase();

            if (db.HashExists("onlineUserHash", profile.DisplayName))
            {
                db.HashIncrement("onlineUserHash", profile.DisplayName, 1);
            }
            else
            {
                db.HashSet("onlineUserHash", profile.DisplayName, 1);
            }

            db.SortedSetAdd("onlineUserSet", profile.DisplayName, DateTime.Now.Ticks);

            var onlineUser = db.SortedSetRangeByScore("onlineUserSet", double.NegativeInfinity, double.PositiveInfinity,
                                                        Exclude.None, Order.Descending).Select(t => t.ToString()).ToArray();

            await _onlineUserHubContext.Clients.All.SendAsync("onlineUser", onlineUser);

            return Ok();
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

        [HttpGet("closeTab")]
        public async Task<IActionResult> OfflineUser()
        {
            Debug.WriteLine("closeTab Request\n");
            var user = await _context.Users.Include(u => u.Profile).FirstOrDefaultAsync(u => u.Id == UserId);
            var userDto = _mapper.Map<UserDto>(user);

            var db = _redisDatabaseProvider.GetDatabase();

            long tab = db.HashDecrement("onlineUserHash", user.Profile.DisplayName);

            if (tab == 0)
            {
                db.SortedSetRemove("onlineUserSet", user.Profile.DisplayName);
            }

            var onlineUser = db.SortedSetRangeByScore("onlineUserSet", double.NegativeInfinity, double.PositiveInfinity,
                                                        Exclude.None, Order.Descending).Select(t => t.ToString()).ToArray();

            await _onlineUserHubContext.Clients.All.SendAsync("onlineUser", onlineUser);

            return Ok(userDto);
        }
    }
}
