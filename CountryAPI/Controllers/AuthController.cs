using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using CountryAPI.Models;
using System.Text.Json;
using CountryAPI.Data;
using CountryAPI.Functions;

namespace CountryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly DataContext _context;
        public AuthController(IConfiguration configuration, DataContext context)
        {
            _configuration = configuration;
            _context = context;
        }


        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email == request.Email);
            
            if (user != null) {
                return BadRequest("Email Already Exist");
            }
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var newUser = new User
            {
                User_name = request.User_name,
                ref_profile = Guid.NewGuid(),
                Email = request.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            string token = TokenManager.GenerateToken(newUser.ref_profile.ToString());
            object userData = new { token, user_data = JsonSerializer.Serialize(newUser.user_obj()) };
            return Ok(userData);
        }

        [HttpPost("login")]
        public async Task<ActionResult<object>> Login(UserDto request)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email == request.Email);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Wrong Password.");
            }

            string token = TokenManager.GenerateToken(user.ref_profile.ToString());
            object userData = new { token, user_data = JsonSerializer.Serialize(user.user_obj()) };
            return Ok(userData);
        }

        [HttpPost("sessioninfo")]
        public async Task<ActionResult<string>> SessionInfo(SessionDto sessonInfo)
        {
            var ref_profile = TokenManager.ValidateToken(sessonInfo.token);
            if(string.IsNullOrEmpty(ref_profile))
            {
                return BadRequest("Something went wrong.");
            }
            var user = _context.Users.FirstOrDefault(x => x.ref_profile == Guid.Parse(ref_profile));

            object userData = new { sessonInfo.token, user_data = JsonSerializer.Serialize(user.user_obj()) };

            return Ok(userData);
        }

   /*     private string CreateToken(User user)
        {
            string jsonString = JsonSerializer.Serialize(user.user_obj());
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.UserData, jsonString),
                new Claim(ClaimTypes.Role, "Admin")
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                 _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
*/
     

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

    }
}
