using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Application.DTOs.Doctor;

public class CreateDoctorDto
{
    [Required] [MaxLength(50)] public string FirstName { get; set; } = string.Empty;
    [Required] [MaxLength(50)] public string LastName { get; set; } = string.Empty;
    [Required] [EmailAddress]  public string Email { get; set; } = string.Empty;
    [Required] [Phone]         public string PhoneNumber { get; set; } = string.Empty;
    [Required]                 public string Specialization { get; set; } = string.Empty;
    [Required]                 public string LicenseNumber { get; set; } = string.Empty;
    [Range(0, 60)]             public int YearsOfExperience { get; set; }
    public string? Bio { get; set; }
    [Range(0, 100000)]         public decimal ConsultationFee { get; set; }
}

public class UpdateDoctorDto
{
    [MaxLength(50)] public string? FirstName { get; set; }
    [MaxLength(50)] public string? LastName { get; set; }
    [Phone]         public string? PhoneNumber { get; set; }
    public string? Specialization { get; set; }
    public string? Bio { get; set; }
    public bool? IsAvailable { get; set; }
    [Range(0, 100000)] public decimal? ConsultationFee { get; set; }
}

public class DoctorResponseDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Specialization { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
    public int YearsOfExperience { get; set; }
    public string? Bio { get; set; }
    public bool IsAvailable { get; set; }
    public decimal ConsultationFee { get; set; }
    public DateTime CreatedAt { get; set; }
}
