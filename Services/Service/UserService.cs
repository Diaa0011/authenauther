using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using authenAutherApp.Dtos.Request;
using authenAutherApp.Dtos.Response;
using authenAutherApp.Response;
using authenAutherApp.Services.IService;
using AuthenAutherApp.Data.AppDbContext;
using AuthenAutherApp.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace authenAutherApp.Services.Service
{
    public class UserService : IUserService
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly DbSet<ApplicationUser> _user;
        private readonly IConfiguration _configuration;
        public UserService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ApplicationDbContext context, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _context = context;

            _user = _context.Users;

        }
        public Task<SignInResponse> SiginAsync(SignInRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<SignUpResponse> SignupAsync(SignUpRequest request)
        {
            Log.Information("Starting user signup process...");
            var user = new ApplicationUser
            {
                FullName = request.UserName,
                Email = request.Email,
                UserName = request.Email,
                EmailConfirmed = false
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            Log.Information("User creation process completed.");

            Log.Error($"request failed: {result.Errors}");

            if (result.Succeeded)
            {
                var token = GenerateJwtToken(user);
                // Here you would typically generate a JWT token
                return new SignUpResponse
                {
                    UserId = user.Id,
                    Message = $"User {user.FullName} created successfully",
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

        public async Task<ResponseModel<List<UserReponse>>> GetAllUsers()
        {
            try
            {
                var response = new List<UserReponse>();
                var users = await _user.ToListAsync();
                if (users != null)
                {
                    response = users.Select(x => new UserReponse { UserId = x.Id, Email = x.UserName }).ToList();

                }
                return new ResponseModel<List<UserReponse>>
                {
                    Success = true,
                    Message = "success",
                    Result = response
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<List<UserReponse>>
                {
                    Success = false,
                    Message = "false",

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