using HospitalManagement.Domain.Entities;
using HospitalManagement.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<MedicalRecord> MedicalRecords => Set<MedicalRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Patient>().HasQueryFilter(p => !p.IsDeleted);
        modelBuilder.Entity<Doctor>().HasQueryFilter(d => !d.IsDeleted);
        modelBuilder.Entity<Appointment>().HasQueryFilter(a => !a.IsDeleted);
        modelBuilder.Entity<MedicalRecord>().HasQueryFilter(m => !m.IsDeleted);

        modelBuilder.Entity<Patient>(e =>
        {
            e.HasKey(p => p.Id);
            e.Property(p => p.FirstName).HasMaxLength(50).IsRequired();
            e.Property(p => p.LastName).HasMaxLength(50).IsRequired();
            e.Property(p => p.Email).HasMaxLength(100).IsRequired();
            e.HasIndex(p => p.Email).IsUnique();
            e.Property(p => p.BloodGroup).HasMaxLength(5);
        });

        modelBuilder.Entity<Doctor>(e =>
        {
            e.HasKey(d => d.Id);
            e.Property(d => d.FirstName).HasMaxLength(50).IsRequired();
            e.Property(d => d.LastName).HasMaxLength(50).IsRequired();
            e.Property(d => d.Email).HasMaxLength(100).IsRequired();
            e.HasIndex(d => d.Email).IsUnique();
            e.Property(d => d.LicenseNumber).HasMaxLength(50).IsRequired();
            e.HasIndex(d => d.LicenseNumber).IsUnique();
            e.Property(d => d.ConsultationFee).HasPrecision(10, 2);
        });

        modelBuilder.Entity<Appointment>(e =>
        {
            e.HasKey(a => a.Id);
            e.Property(a => a.Status).HasConversion<string>();
            e.Property(a => a.Fee).HasPrecision(10, 2);
            e.HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<MedicalRecord>(e =>
        {
            e.HasKey(m => m.Id);
            e.Property(m => m.Diagnosis).HasMaxLength(500).IsRequired();
            e.HasOne(m => m.Patient)
                .WithMany(p => p.MedicalRecords)
                .HasForeignKey(m => m.PatientId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(m => m.Doctor)
                .WithMany(d => d.MedicalRecords)
                .HasForeignKey(m => m.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Modified))
        {
            if (entry.Entity is Domain.Common.BaseEntity entity)
                entity.UpdatedAt = DateTime.UtcNow;
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}
