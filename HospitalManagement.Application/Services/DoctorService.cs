using HospitalManagement.Application.Common;
using HospitalManagement.Application.DTOs.Doctor;
using HospitalManagement.Application.Interfaces;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces;

namespace HospitalManagement.Application.Services;

public class DoctorService : IDoctorService
{
    private readonly IDoctorRepository _doctorRepository;

    public DoctorService(IDoctorRepository doctorRepository)
    {
        _doctorRepository = doctorRepository;
    }

    public async Task<ApiResponse<DoctorResponseDto>> GetByIdAsync(Guid id)
    {
        var doctor = await _doctorRepository.GetByIdAsync(id);
        if (doctor == null)
            return ApiResponse<DoctorResponseDto>.Fail($"Doctor with ID {id} not found.");
        return ApiResponse<DoctorResponseDto>.Ok(MapToDto(doctor));
    }

    public async Task<ApiResponse<IEnumerable<DoctorResponseDto>>> GetAllAsync()
    {
        var doctors = await _doctorRepository.GetAllAsync();
        return ApiResponse<IEnumerable<DoctorResponseDto>>.Ok(doctors.Select(MapToDto));
    }

    public async Task<ApiResponse<DoctorResponseDto>> CreateAsync(CreateDoctorDto dto)
    {
        var existing = await _doctorRepository.GetByEmailAsync(dto.Email);
        if (existing != null)
            return ApiResponse<DoctorResponseDto>.Fail("A doctor with this email already exists.");

        var doctor = new Doctor
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            Specialization = dto.Specialization,
            LicenseNumber = dto.LicenseNumber,
            YearsOfExperience = dto.YearsOfExperience,
            Bio = dto.Bio,
            ConsultationFee = dto.ConsultationFee
        };

        var created = await _doctorRepository.AddAsync(doctor);
        return ApiResponse<DoctorResponseDto>.Ok(MapToDto(created), "Doctor registered successfully.");
    }

    public async Task<ApiResponse<DoctorResponseDto>> UpdateAsync(Guid id, UpdateDoctorDto dto)
    {
        var doctor = await _doctorRepository.GetByIdAsync(id);
        if (doctor == null)
            return ApiResponse<DoctorResponseDto>.Fail($"Doctor with ID {id} not found.");

        if (dto.FirstName != null) doctor.FirstName = dto.FirstName;
        if (dto.LastName != null) doctor.LastName = dto.LastName;
        if (dto.PhoneNumber != null) doctor.PhoneNumber = dto.PhoneNumber;
        if (dto.Specialization != null) doctor.Specialization = dto.Specialization;
        if (dto.Bio != null) doctor.Bio = dto.Bio;
        if (dto.IsAvailable.HasValue) doctor.IsAvailable = dto.IsAvailable.Value;
        if (dto.ConsultationFee.HasValue) doctor.ConsultationFee = dto.ConsultationFee.Value;

        doctor.UpdatedAt = DateTime.UtcNow;
        await _doctorRepository.UpdateAsync(doctor);
        return ApiResponse<DoctorResponseDto>.Ok(MapToDto(doctor), "Doctor updated successfully.");
    }

    public async Task<ApiResponse<bool>> DeleteAsync(Guid id)
    {
        var exists = await _doctorRepository.ExistsAsync(id);
        if (!exists)
            return ApiResponse<bool>.Fail($"Doctor with ID {id} not found.");
        await _doctorRepository.DeleteAsync(id);
        return ApiResponse<bool>.Ok(true, "Doctor deleted successfully.");
    }

    public async Task<ApiResponse<IEnumerable<DoctorResponseDto>>> GetBySpecializationAsync(string specialization)
    {
        var doctors = await _doctorRepository.GetBySpecializationAsync(specialization);
        return ApiResponse<IEnumerable<DoctorResponseDto>>.Ok(doctors.Select(MapToDto));
    }

    public async Task<ApiResponse<IEnumerable<DoctorResponseDto>>> GetAvailableDoctorsAsync()
    {
        var doctors = await _doctorRepository.GetAvailableDoctorsAsync();
        return ApiResponse<IEnumerable<DoctorResponseDto>>.Ok(doctors.Select(MapToDto));
    }

    private static DoctorResponseDto MapToDto(Doctor d) => new()
    {
        Id = d.Id,
        FullName = d.FullName,
        Email = d.Email,
        PhoneNumber = d.PhoneNumber,
        Specialization = d.Specialization,
        LicenseNumber = d.LicenseNumber,
        YearsOfExperience = d.YearsOfExperience,
        Bio = d.Bio,
        IsAvailable = d.IsAvailable,
        ConsultationFee = d.ConsultationFee,
        CreatedAt = d.CreatedAt
    };
}
