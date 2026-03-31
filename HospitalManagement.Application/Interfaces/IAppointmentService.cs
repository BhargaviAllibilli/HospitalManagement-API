using HospitalManagement.Application.Common;
using HospitalManagement.Application.DTOs.Appointment;

namespace HospitalManagement.Application.Interfaces;

public interface IAppointmentService
{
    Task<ApiResponse<AppointmentResponseDto>> GetByIdAsync(Guid id);
    Task<ApiResponse<IEnumerable<AppointmentResponseDto>>> GetAllAsync();
    Task<ApiResponse<AppointmentResponseDto>> CreateAsync(CreateAppointmentDto dto);
    Task<ApiResponse<AppointmentResponseDto>> UpdateAsync(Guid id, UpdateAppointmentDto dto);
    Task<ApiResponse<bool>> CancelAsync(Guid id);
    Task<ApiResponse<IEnumerable<AppointmentResponseDto>>> GetByPatientIdAsync(Guid patientId);
    Task<ApiResponse<IEnumerable<AppointmentResponseDto>>> GetByDoctorIdAsync(Guid doctorId);
}

