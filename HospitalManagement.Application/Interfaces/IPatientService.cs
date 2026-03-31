using HospitalManagement.Application.Common;
using HospitalManagement.Application.DTOs.Patient;

namespace HospitalManagement.Application.Interfaces;

public interface IPatientService
{
    Task<ApiResponse<PatientResponseDto>> GetByIdAsync(Guid id);
    Task<ApiResponse<IEnumerable<PatientResponseDto>>> GetAllAsync();
    Task<ApiResponse<PatientResponseDto>> CreateAsync(CreatePatientDto dto);
    Task<ApiResponse<PatientResponseDto>> UpdateAsync(Guid id, UpdatePatientDto dto);
    Task<ApiResponse<bool>> DeleteAsync(Guid id);
    Task<ApiResponse<PatientResponseDto>> GetByEmailAsync(string email);
}

