using AgriculturalTech.API.Data.Models;
using AgriculturalTech.API.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.UI.Services;
using AgriculturalTech.API.Services.Implementations;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AgriculturalTech.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IExtendedEmailSender _emailSender;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration, IExtendedEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _emailSender = emailSender;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<string>>> Register([FromBody] RegisterDto model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName,
                FarmName = model.FarmName,
                FarmLocation = model.FarmLocation,
                PreferredLanguage = model.PreferredLanguage ?? "en"
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Farmer");

                await _emailSender.SendVerificationCode(user);

                return Ok(ApiResponse<string>.SuccessResponse(user.Email, "Registration successful. Please verify your email using the code sent to you"));
            }
            var errors = result.Errors.Select(e => e.Description).ToList();
            return BadRequest(ApiResponse<string>.ErrorResponse("Registration failed", errors));
        }

        [HttpPost("verify-email")]
        public async Task<ActionResult<ApiResponse<string>>> VerifyEmail([FromBody] VerifyEmailDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return NotFound(ApiResponse<string>.ErrorResponse("User not found."));

            if (user.EmailConfirmed)
                return BadRequest(ApiResponse<string>.ErrorResponse("Email already verified."));

            var isVerified = await _emailSender.VerifyCode(user, model.Code);

            if (isVerified)
            {
                user.EmailConfirmed = true;

                await _userManager.UpdateAsync(user);

                var token = GenerateJwtToken(user);

                return Ok(ApiResponse<string>.SuccessResponse(token, "Email verified successfully."));
            }

            return BadRequest(ApiResponse<string>.ErrorResponse("Invalid verification code."));
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<LoginResponseDto>>> Login([FromBody] LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
                return Unauthorized(ApiResponse<LoginResponseDto>.ErrorResponse("Invalid credentials"));

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (result.Succeeded)
            {
                user.LastLoginAt = DateTime.UtcNow;
                await _userManager.UpdateAsync(user);

                var token = GenerateJwtToken(user);
                var response = new LoginResponseDto
                {
                    Token = token,
                    Email = user.Email,
                    FullName = user.FullName,
                    FarmName = user.FarmName
                };

                return Ok(ApiResponse<LoginResponseDto>.SuccessResponse(response, "Login successful"));
            }

            return Unauthorized(ApiResponse<LoginResponseDto>.ErrorResponse("Invalid credentials"));
        }

        [HttpPost("logout")]
        public async Task<ActionResult<ApiResponse<string>>> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(ApiResponse<string>.SuccessResponse(null, "Logout successful"));
        }

        [HttpPost("Emails")]
        public async Task<ActionResult<ApiResponse<string>>> SendEmail([FromBody] EmailSendDto model)
        {
            try
            {
                await _emailSender.SendEmailAsync(model.To, model.Subject, model.Body);
                return Ok(ApiResponse<string>.SuccessResponse(null, "Email sent successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.ErrorResponse("Failed to send email", new List<string> { ex.Message }));
            }
        }


        private string GenerateJwtToken(ApplicationUser user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(Convert.ToDouble(_configuration["Jwt:ExpiryInHours"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
