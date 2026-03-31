using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces;
using HospitalManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Infrastructure.Repositories;

public class PatientRepository : GenericRepository<Patient>, IPatientRepository
{
    public PatientRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Patient?> GetByEmailAsync(string email)
        => await _dbSet.FirstOrDefaultAsync(p => p.Email == email);

    public async Task<IEnumerable<Patient>> GetPatientsWithAppointmentsAsync()
        => await _dbSet.Include(p => p.Appointments).ToListAsync();

    public async Task<Patient?> GetPatientWithMedicalHistoryAsync(Guid patientId)
        => await _dbSet
            .Include(p => p.MedicalRecords)
            .Include(p => p.Appointments)
            .FirstOrDefaultAsync(p => p.Id == patientId);
}
