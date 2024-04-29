using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;

namespace Min_API_DZ.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private List<UserDto> _users = new List<UserDto>
        {
            new UserDto { UserName = "user1", Password = "pass1" },
            new UserDto { UserName = "user2", Password = "pass2" },
            new UserDto { UserName = "user3", Password = "pass3" }
        };

        public AuthController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public class LoginModel
        {
            [Required]
            public string UserName { get; set; }

            [Required]
            public string Password { get; set; }
        }
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel login)
        {
            var user = _users.FirstOrDefault(u => u.UserName == login.UserName && u.Password == login.Password);
            if (user != null)
            {
                var token = _tokenService.BuildToken("key-1111111111111111111111111111111111111111111111111111111111111111111", "example.com", user);
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
                var expiration = securityToken.ValidTo;

                return Ok(new
                {
                    Message = "Authentication successful",
                    Token = token,
                    Expires = expiration
                });
            }
            return Unauthorized("Invalid credentials");
        }
    }
}
