using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces;
using HospitalManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Infrastructure.Repositories;

public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
{
    public AppointmentRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<Appointment>> GetByPatientIdAsync(Guid patientId)
        => await _dbSet
            .Include(a => a.Doctor)
            .Include(a => a.Patient)
            .Where(a => a.PatientId == patientId)
            .ToListAsync();

    public async Task<IEnumerable<Appointment>> GetByDoctorIdAsync(Guid doctorId)
        => await _dbSet
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .Where(a => a.DoctorId == doctorId)
            .ToListAsync();

    public async Task<IEnumerable<Appointment>> GetByDateAsync(DateTime date)
        => await _dbSet
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .Where(a => a.AppointmentDate.Date == date.Date)
            .ToListAsync();

    public async Task<bool> HasConflictAsync(Guid doctorId, DateTime date, TimeSpan start, TimeSpan end, Guid? excludeId = null)
        => await _dbSet.AnyAsync(a =>
            a.DoctorId == doctorId &&
            a.AppointmentDate.Date == date.Date &&
            a.Status != AppointmentStatus.Cancelled &&
            (excludeId == null || a.Id != excludeId) &&
            a.StartTime < end &&
            a.EndTime > start);
}
