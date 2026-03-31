using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces;
using HospitalManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Infrastructure.Repositories;

public class MedicalRecordRepository : GenericRepository<MedicalRecord>, IMedicalRecordRepository
{
    public MedicalRecordRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<MedicalRecord>> GetByPatientIdAsync(Guid patientId)
        => await _dbSet
            .Include(m => m.Doctor)
            .Where(m => m.PatientId == patientId)
            .OrderByDescending(m => m.RecordDate)
            .ToListAsync();

    public async Task<IEnumerable<MedicalRecord>> GetByDoctorIdAsync(Guid doctorId)
        => await _dbSet
            .Include(m => m.Patient)
            .Where(m => m.DoctorId == doctorId)
            .OrderByDescending(m => m.RecordDate)
            .ToListAsync();
}