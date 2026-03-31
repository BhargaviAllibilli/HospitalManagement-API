using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Application.DTOs.Auth;

public class RegisterDto
{
    [Required] [MaxLength(50)] public string FirstName { get; set; } = string.Empty;
    [Required] [MaxLength(50)] public string LastName { get; set; } = string.Empty;
    [Required] [EmailAddress]  public string Email { get; set; } = string.Empty;
    [Required] [MinLength(6)]  public string Password { get; set; } = string.Empty;
    [Required]                 public string Role { get; set; } = "Patient";
}

public class LoginDto
{
    [Required] [EmailAddress] public string Email { get; set; } = string.Empty;
    [Required]                public string Password { get; set; } = string.Empty;
}

public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime Expiry { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}