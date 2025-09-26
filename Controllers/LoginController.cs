using ECPAPI.Data;
using ECPAPI.Models;
using ECPAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ECPAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public LoginController(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        [HttpPost]
        public async Task<ActionResult> LoginAsync(LoginDTO model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest("Please Provide UserNameOrEmail and Password");
            }

            //LoginResponseDTO response = new() { UserNameOrEmail = model.UserNameOrEmail };
            var user = await _userService.AuthenticateAsync(model.UserNameOrEmail, model.Password);
            if (user == null)
                return Unauthorized("Invalid username or password");

            //if (model.UserNameOrEmail == "User" && model.Password == "1234")

            byte[] key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JWTSecretForLocal"));
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                        new Claim(ClaimTypes.Name,model.UserNameOrEmail),
                        new Claim(ClaimTypes.Role,"Admin")
                }),

                Issuer = "TechLover",
                Audience = "TechLover",

                Expires = DateTime.Now.AddHours(4),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var response = new LoginResponseDTO
            {
                UserNameOrEmail = user.UserNameOrEmail,
                Token = tokenHandler.WriteToken(token)
            };
            
                return Ok(response);
        }
    }
}

        //var token = tokenHandler.CreateToken(tokenDescriptor);
        //response.Token = tokenHandler.WriteToken(token);
 

//        private readonly IConfiguration _configuration;
//        private readonly ECPDbContext _context;

//        public LoginController(IConfiguration configuration, ECPDbContext context)
//        {
//            _configuration = configuration;
//            _context = context;
//        }
//        [HttpPost]
//        public async Task<IActionResult> Login([FromBody] LoginDTO model)
//        {
//            if (!ModelState.IsValid)
//            {//  return BadRequest("Please Provide UserName and Password");}

//            var user = await _context.Users.FirstOrDefaultAsync(u => (u.UserName == model.UserNameOrEmail || u.Email == model.UserNameOrEmail)
//            && u.Password == model.Password);

//            if (user == null)
//                return Unauthorized(new { message = "Invalid username/email or password" });
//            var tokenHandler = new JwtSecurityTokenHandler();
//            var key = Encoding.ASCII.GetBytes(_configuration["JWTSecretForLocal"]);

//            var tokenDescriptor = new SecurityTokenDescriptor
//            {
//                Subject = new ClaimsIdentity(new[]
//                {
//                    new Claim(ClaimTypes.Name, user.UserName),
//                    new Claim(ClaimTypes.Email, user.Email),
//                    new Claim(ClaimTypes.Role, "Admin") // ან User თუ გინდა
//                }),
//                Expires = DateTime.UtcNow.AddHours(4),
//                Issuer = "TechLover",
//                Audience = "TechLover",
//                SigningCredentials = new SigningCredentials(
//                    new SymmetricSecurityKey(key),
//                    SecurityAlgorithms.HmacSha512Signature
//                )
//            };
//            var token = tokenHandler.CreateToken(tokenDescriptor);
//            return Ok(new LoginResponseDTO
//            {
//                UserName = user.UserName,
//                Token = tokenHandler.WriteToken(token)
//            });

//            var hasher = new PasswordHasher<User>();
//            var User = new User();
//            string password = "Lasha123";
//            string passwordHash = hasher.HashPassword(user, password);
//            Console.WriteLine(passwordHash);
//        }}}

////(model.UserNameOrEmail == "User123" && model.Password == "1234")
////    {byte[] key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JWTSecretForLocal"));
////        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
////        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
////        {Subject = new ClaimsIdentity(new Claim[]
////            {new Claim(ClaimTypes.Name,model.UserNameOrEmail ),
////             new Claim(ClaimTypes.Role,"Admin")}),
////            Issuer = "TechLover",
////            Audience = "TechLover",
////            Expires = DateTime.Now.AddHours(4),
////            SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)};
////        var token = tokenHandler.CreateToken(tokenDescriptor);
////        response.Token = tokenHandler.WriteToken(token);}
////    else { return Ok("Invalid username or password");}
////    return Ok(response);}}}
