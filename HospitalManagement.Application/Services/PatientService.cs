using HospitalManagement.Application.Common;
using HospitalManagement.Application.DTOs.Patient;
using HospitalManagement.Application.Interfaces;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces;

namespace HospitalManagement.Application.Services;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepository;

    public PatientService(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }

    public async Task<ApiResponse<PatientResponseDto>> GetByIdAsync(Guid id)
    {
        var patient = await _patientRepository.GetByIdAsync(id);
        if (patient == null)
            return ApiResponse<PatientResponseDto>.Fail($"Patient with ID {id} not found.");
        return ApiResponse<PatientResponseDto>.Ok(MapToDto(patient));
    }

    public async Task<ApiResponse<IEnumerable<PatientResponseDto>>> GetAllAsync()
    {
        var patients = await _patientRepository.GetAllAsync();
        return ApiResponse<IEnumerable<PatientResponseDto>>.Ok(patients.Select(MapToDto));
    }

    public async Task<ApiResponse<PatientResponseDto>> CreateAsync(CreatePatientDto dto)
    {
        var existing = await _patientRepository.GetByEmailAsync(dto.Email);
        if (existing != null)
            return ApiResponse<PatientResponseDto>.Fail("A patient with this email already exists.");

        var patient = new Patient
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            DateOfBirth = dto.DateOfBirth,
            Gender = dto.Gender,
            Address = dto.Address,
            BloodGroup = dto.BloodGroup,
            EmergencyContactName = dto.EmergencyContactName,
            EmergencyContactPhone = dto.EmergencyContactPhone
        };

        var created = await _patientRepository.AddAsync(patient);
        return ApiResponse<PatientResponseDto>.Ok(MapToDto(created), "Patient registered successfully.");
    }

    public async Task<ApiResponse<PatientResponseDto>> UpdateAsync(Guid id, UpdatePatientDto dto)
    {
        var patient = await _patientRepository.GetByIdAsync(id);
        if (patient == null)
            return ApiResponse<PatientResponseDto>.Fail($"Patient with ID {id} not found.");

        if (dto.FirstName != null) patient.FirstName = dto.FirstName;
        if (dto.LastName != null) patient.LastName = dto.LastName;
        if (dto.PhoneNumber != null) patient.PhoneNumber = dto.PhoneNumber;
        if (dto.Address != null) patient.Address = dto.Address;
        if (dto.EmergencyContactName != null) patient.EmergencyContactName = dto.EmergencyContactName;
        if (dto.EmergencyContactPhone != null) patient.EmergencyContactPhone = dto.EmergencyContactPhone;

        patient.UpdatedAt = DateTime.UtcNow;
        await _patientRepository.UpdateAsync(patient);
        return ApiResponse<PatientResponseDto>.Ok(MapToDto(patient), "Patient updated successfully.");
    }

    public async Task<ApiResponse<bool>> DeleteAsync(Guid id)
    {
        var exists = await _patientRepository.ExistsAsync(id);
        if (!exists)
            return ApiResponse<bool>.Fail($"Patient with ID {id} not found.");
        await _patientRepository.DeleteAsync(id);
        return ApiResponse<bool>.Ok(true, "Patient deleted successfully.");
    }

    public async Task<ApiResponse<PatientResponseDto>> GetByEmailAsync(string email)
    {
        var patient = await _patientRepository.GetByEmailAsync(email);
        if (patient == null)
            return ApiResponse<PatientResponseDto>.Fail($"No patient found with email {email}.");
        return ApiResponse<PatientResponseDto>.Ok(MapToDto(patient));
    }

    private static PatientResponseDto MapToDto(Patient p)
{
    return new PatientResponseDto
    {
        Id = p.Id,
        FullName = p.FullName,
        Email = p.Email,
        PhoneNumber = p.PhoneNumber,
        Age = p.Age,
        Gender = p.Gender,
        BloodGroup = p.BloodGroup,
        Address = p.Address,
        EmergencyContactName = p.EmergencyContactName,
        EmergencyContactPhone = p.EmergencyContactPhone,
        CreatedAt = p.CreatedAt
    };
}
}