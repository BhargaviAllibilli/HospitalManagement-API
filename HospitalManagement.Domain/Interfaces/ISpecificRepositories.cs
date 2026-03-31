using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Domain.Interfaces;

public interface IPatientRepository : IRepository<Patient>
{
    Task<Patient?> GetByEmailAsync(string email);
    Task<IEnumerable<Patient>> GetPatientsWithAppointmentsAsync();
    Task<Patient?> GetPatientWithMedicalHistoryAsync(Guid patientId);
}

public interface IDoctorRepository : IRepository<Doctor>
{
    Task<Doctor?> GetByEmailAsync(string email);
    Task<IEnumerable<Doctor>> GetBySpecializationAsync(string specialization);
    Task<IEnumerable<Doctor>> GetAvailableDoctorsAsync();
    Task<Doctor?> GetDoctorWithAppointmentsAsync(Guid doctorId);
}

public interface IAppointmentRepository : IRepository<Appointment>
{
    Task<IEnumerable<Appointment>> GetByPatientIdAsync(Guid patientId);
    Task<IEnumerable<Appointment>> GetByDoctorIdAsync(Guid doctorId);
    Task<IEnumerable<Appointment>> GetByDateAsync(DateTime date);
    Task<bool> HasConflictAsync(Guid doctorId, DateTime date, TimeSpan start, TimeSpan end, Guid? excludeId = null);
}

public interface IMedicalRecordRepository : IRepository<MedicalRecord>
{
    Task<IEnumerable<MedicalRecord>> GetByPatientIdAsync(Guid patientId);
    Task<IEnumerable<MedicalRecord>> GetByDoctorIdAsync(Guid doctorId);
}