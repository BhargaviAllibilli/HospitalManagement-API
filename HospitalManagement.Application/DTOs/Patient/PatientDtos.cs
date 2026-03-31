using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Application.DTOs.Patient;

public class CreatePatientDto
{
    [Required] [MaxLength(50)] public string FirstName { get; set; } = string.Empty;
    [Required] [MaxLength(50)] public string LastName { get; set; } = string.Empty;
    [Required] [EmailAddress]  public string Email { get; set; } = string.Empty;
    [Required] [Phone]         public string PhoneNumber { get; set; } = string.Empty;
    [Required]                 public DateTime DateOfBirth { get; set; }
    [Required]                 public string Gender { get; set; } = string.Empty;
    [MaxLength(200)]           public string Address { get; set; } = string.Empty;
    [MaxLength(5)]             public string BloodGroup { get; set; } = string.Empty;
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhone { get; set; }
}

public class UpdatePatientDto
{
    [MaxLength(50)] public string? FirstName { get; set; }
    [MaxLength(50)] public string? LastName { get; set; }
    [Phone]         public string? PhoneNumber { get; set; }
    [MaxLength(200)] public string? Address { get; set; }
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhone { get; set; }
}

public class PatientResponseDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Gender { get; set; } = string.Empty;
    public string BloodGroup { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhone { get; set; }
    public DateTime CreatedAt { get; set; }
}
