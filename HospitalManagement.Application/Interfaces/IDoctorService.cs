using HospitalManagement.Application.Common;
using HospitalManagement.Application.DTOs.Doctor;

namespace HospitalManagement.Application.Interfaces;

public interface IDoctorService
{
    Task<ApiResponse<DoctorResponseDto>> GetByIdAsync(Guid id);
    Task<ApiResponse<IEnumerable<DoctorResponseDto>>> GetAllAsync();
    Task<ApiResponse<DoctorResponseDto>> CreateAsync(CreateDoctorDto dto);
    Task<ApiResponse<DoctorResponseDto>> UpdateAsync(Guid id, UpdateDoctorDto dto);
    Task<ApiResponse<bool>> DeleteAsync(Guid id);
    Task<ApiResponse<IEnumerable<DoctorResponseDto>>> GetBySpecializationAsync(string specialization);
    Task<ApiResponse<IEnumerable<DoctorResponseDto>>> GetAvailableDoctorsAsync();
}