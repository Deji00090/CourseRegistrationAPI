using CourseRegistrationAPI.DTO;
using CourseRegistrationAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CourseRegistrationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager; // used for managing users
        private readonly SignInManager<ApplicationUser> signInManager; // provide api for user sign in
        private readonly IConfiguration configuration;

        public AuthController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, 
            IConfiguration configuration)
        {
            _userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel model )
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    Log.Warning("Invalid model state in Register API");
                    return BadRequest(ModelState);
                }

                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Fullname = model.FullName,
                    IsInstructor = model.IsInstructor
                };

                var result = await _userManager.CreateAsync(user, model.Password!);
                if (!result.Succeeded)
                {
                    Log.Warning("User registration failed : {Errors} ", result.Errors);
                    return BadRequest(result.Errors);
                }

                var role = model.IsInstructor ? "Instructor" : "Student";
                await _userManager.AddToRoleAsync(user, role);

                Log.Information("user {Email} registered successfully as {Role}", user.Email, role);
                return Ok(new { message = "User registered successful!" });
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in registration ocurred..please try again later");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> login(LoginModel login)
        {
            var user =  await _userManager.FindByEmailAsync(login.Email!);
            if (user is null)
            {
                Log.Warning("Unable to find {Email} :", login.Email);
                return BadRequest("Invalid email");
            }

            var result =  await signInManager.PasswordSignInAsync(user, login.Password!,false,false);
            if (!result.Succeeded)
            {
                Log.Warning("Invalid  {Email} / {Result}:", login.Email, result);
                return Unauthorized("Invalid email or password");
            }

            var token = GenerateToken(user);
            Log.Information("Token {token}  genearted for {id}", token, user);
            return Ok(new { token });
        }

        private string GenerateToken(ApplicationUser user)
        {
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));

            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

            var claims = new  List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.Id), //id 
                new Claim(JwtRegisteredClaimNames.Email,user.Email!),              
                new Claim(ClaimTypes.Role,(bool)user.IsInstructor! ? "Instructor" : "Student"),
                new Claim("Fullname",user.Fullname!)
            };

            var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audinece"],
                claims : claims,
                expires : DateTime.UtcNow.AddHours(2),
                signingCredentials : credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpGet("test-error")]
        public IActionResult testerror()
        {
            throw new Exception("this is a test exception");
        }
    }
}


