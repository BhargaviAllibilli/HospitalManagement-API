using HospitalManagement.Application.Common;
using HospitalManagement.Application.DTOs.Appointment;
using HospitalManagement.Application.Interfaces;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces;

namespace HospitalManagement.Application.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly IDoctorRepository _doctorRepository;

    public AppointmentService(
        IAppointmentRepository appointmentRepository,
        IPatientRepository patientRepository,
        IDoctorRepository doctorRepository)
    {
        _appointmentRepository = appointmentRepository;
        _patientRepository = patientRepository;
        _doctorRepository = doctorRepository;
    }

    public async Task<ApiResponse<AppointmentResponseDto>> GetByIdAsync(Guid id)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(id);
        if (appointment == null)
            return ApiResponse<AppointmentResponseDto>.Fail($"Appointment with ID {id} not found.");
        return ApiResponse<AppointmentResponseDto>.Ok(MapToDto(appointment));
    }

    public async Task<ApiResponse<IEnumerable<AppointmentResponseDto>>> GetAllAsync()
    {
        var appointments = await _appointmentRepository.GetAllAsync();
        return ApiResponse<IEnumerable<AppointmentResponseDto>>.Ok(appointments.Select(MapToDto));
    }

    public async Task<ApiResponse<AppointmentResponseDto>> CreateAsync(CreateAppointmentDto dto)
    {
        var patient = await _patientRepository.GetByIdAsync(dto.PatientId);
        if (patient == null)
            return ApiResponse<AppointmentResponseDto>.Fail("Patient not found.");

        var doctor = await _doctorRepository.GetByIdAsync(dto.DoctorId);
        if (doctor == null)
            return ApiResponse<AppointmentResponseDto>.Fail("Doctor not found.");
        if (!doctor.IsAvailable)
            return ApiResponse<AppointmentResponseDto>.Fail("Doctor is currently not available.");

        if (dto.StartTime >= dto.EndTime)
            return ApiResponse<AppointmentResponseDto>.Fail("Start time must be before end time.");

        var hasConflict = await _appointmentRepository.HasConflictAsync(
            dto.DoctorId, dto.AppointmentDate, dto.StartTime, dto.EndTime);
        if (hasConflict)
            return ApiResponse<AppointmentResponseDto>.Fail("Doctor already has an appointment in this time slot.");

        var appointment = new Appointment
        {
            PatientId = dto.PatientId,
            DoctorId = dto.DoctorId,
            AppointmentDate = dto.AppointmentDate.Date,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            Reason = dto.Reason,
            Fee = doctor.ConsultationFee,
            Status = AppointmentStatus.Scheduled,
            Patient = patient,
            Doctor = doctor
        };

        var created = await _appointmentRepository.AddAsync(appointment);
        return ApiResponse<AppointmentResponseDto>.Ok(MapToDto(created), "Appointment booked successfully.");
    }

    public async Task<ApiResponse<AppointmentResponseDto>> UpdateAsync(Guid id, UpdateAppointmentDto dto)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(id);
        if (appointment == null)
            return ApiResponse<AppointmentResponseDto>.Fail($"Appointment with ID {id} not found.");
        if (appointment.Status == AppointmentStatus.Cancelled)
            return ApiResponse<AppointmentResponseDto>.Fail("Cannot update a cancelled appointment.");

        if (dto.AppointmentDate.HasValue) appointment.AppointmentDate = dto.AppointmentDate.Value.Date;
        if (dto.StartTime.HasValue) appointment.StartTime = dto.StartTime.Value;
        if (dto.EndTime.HasValue) appointment.EndTime = dto.EndTime.Value;
        if (dto.Status.HasValue) appointment.Status = dto.Status.Value;
        if (dto.Notes != null) appointment.Notes = dto.Notes;

        appointment.UpdatedAt = DateTime.UtcNow;
        await _appointmentRepository.UpdateAsync(appointment);
        return ApiResponse<AppointmentResponseDto>.Ok(MapToDto(appointment), "Appointment updated.");
    }

    public async Task<ApiResponse<bool>> CancelAsync(Guid id)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(id);
        if (appointment == null)
            return ApiResponse<bool>.Fail($"Appointment with ID {id} not found.");
        if (appointment.Status == AppointmentStatus.Cancelled)
            return ApiResponse<bool>.Fail("Appointment is already cancelled.");

        appointment.Status = AppointmentStatus.Cancelled;
        appointment.UpdatedAt = DateTime.UtcNow;
        await _appointmentRepository.UpdateAsync(appointment);
        return ApiResponse<bool>.Ok(true, "Appointment cancelled successfully.");
    }

    public async Task<ApiResponse<IEnumerable<AppointmentResponseDto>>> GetByPatientIdAsync(Guid patientId)
    {
        var appointments = await _appointmentRepository.GetByPatientIdAsync(patientId);
        return ApiResponse<IEnumerable<AppointmentResponseDto>>.Ok(appointments.Select(MapToDto));
    }

    public async Task<ApiResponse<IEnumerable<AppointmentResponseDto>>> GetByDoctorIdAsync(Guid doctorId)
    {
        var appointments = await _appointmentRepository.GetByDoctorIdAsync(doctorId);
        return ApiResponse<IEnumerable<AppointmentResponseDto>>.Ok(appointments.Select(MapToDto));
    }

    private static AppointmentResponseDto MapToDto(Appointment a) => new()
    {
        Id = a.Id,
        PatientId = a.PatientId,
        PatientName = a.Patient?.FullName ?? string.Empty,
        DoctorId = a.DoctorId,
        DoctorName = a.Doctor?.FullName ?? string.Empty,
        DoctorSpecialization = a.Doctor?.Specialization ?? string.Empty,
        AppointmentDate = a.AppointmentDate,
        StartTime = a.StartTime,
        EndTime = a.EndTime,
        Status = a.Status.ToString(),
        Reason = a.Reason,
        Notes = a.Notes,
        IsPaid = a.IsPaid,
        Fee = a.Fee,
        CreatedAt = a.CreatedAt
    };
}