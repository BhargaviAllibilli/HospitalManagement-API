using System.ComponentModel.DataAnnotations;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Application.DTOs.Appointment;

public class CreateAppointmentDto
{
    [Required] public Guid PatientId { get; set; }
    [Required] public Guid DoctorId { get; set; }
    [Required] public DateTime AppointmentDate { get; set; }
    [Required] public TimeSpan StartTime { get; set; }
    [Required] public TimeSpan EndTime { get; set; }
    public string? Reason { get; set; }
}

public class UpdateAppointmentDto
{
    public DateTime? AppointmentDate { get; set; }
    public TimeSpan? StartTime { get; set; }
    public TimeSpan? EndTime { get; set; }
    public AppointmentStatus? Status { get; set; }
    public string? Notes { get; set; }
}

public class AppointmentResponseDto
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public Guid DoctorId { get; set; }
    public string DoctorName { get; set; } = string.Empty;
    public string DoctorSpecialization { get; set; } = string.Empty;
    public DateTime AppointmentDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Reason { get; set; }
    public string? Notes { get; set; }
    public bool IsPaid { get; set; }
    public decimal? Fee { get; set; }
    public DateTime CreatedAt { get; set; }
}
