using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces;
using HospitalManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Infrastructure.Repositories;

public class DoctorRepository : GenericRepository<Doctor>, IDoctorRepository
{
    public DoctorRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Doctor?> GetByEmailAsync(string email)
        => await _dbSet.FirstOrDefaultAsync(d => d.Email == email);

    public async Task<IEnumerable<Doctor>> GetBySpecializationAsync(string specialization)
        => await _dbSet.Where(d => d.Specialization == specialization).ToListAsync();

    public async Task<IEnumerable<Doctor>> GetAvailableDoctorsAsync()
        => await _dbSet.Where(d => d.IsAvailable).ToListAsync();

    public async Task<Doctor?> GetDoctorWithAppointmentsAsync(Guid doctorId)
        => await _dbSet
            .Include(d => d.Appointments)
            .FirstOrDefaultAsync(d => d.Id == doctorId);
}
