using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using authenAutherApp.Dtos.Request;
using authenAutherApp.Dtos.Response;
using authenAutherApp.Response;
using authenAutherApp.Services.IService;
using AuthenAutherApp.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace authenAutherApp.Services.Service
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;
        public UserService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }
        public Task<SignInResponse> SiginAsync(SignInRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<SignUpResponse> SignupAsync(SignUpRequest request)
        {
            var user = new ApplicationUser
            {
                FullName = request.UserName,
                Email = request.Email,
                EmailConfirmed = false
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                var token = GenerateJwtToken(user);
                // Here you would typically generate a JWT token
                return new SignUpResponse
                {
                    UserId = user.Id,
                    Message = "User created successfully",
                    Token = token
                };
            }
            else
            {
                return new SignUpResponse
                {
                    UserId = null,
                    Message = "User creation failed",
                    Token = null
                };
            }
        }



        private string GenerateJwtToken(IdentityUser user)
        {
            // Implement JWT token generation logic here
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email)
            };

            // roles will be here

            // Generate the token using claims,issuer,adience,securitykey and return it
            var tokenExpiration = DateTime.UtcNow.AddHours(24);
            var issuer = _configuration["Authentication:Issuer"];
            var audience = _configuration["Authentication:Audience"];
            var securityKey = _configuration["Authentication:Key"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var token = new JwtSecurityToken
            (
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: tokenExpiration,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}