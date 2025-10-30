using System.ComponentModel.DataAnnotations;

namespace AgriculturalTech.API.DTOs
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }

        [Required]
        public string FullName { get; set; }

        public string FarmName { get; set; }
        public string FarmLocation { get; set; }
        public string PreferredLanguage { get; set; } = "en";
    }

    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class LoginResponseDto
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string FarmName { get; set; }
    }

    public class EmailSendDto
    {
        [Required]
        public string To { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Body { get; set; }
    }

    public class VerifyEmailDto
    {
        public string Email { get; set; } = default!;
        public string Code { get; set; } = default!;
    }
}