using Microsoft.EntityFrameworkCore;
using HairSalonManagement.Models;

namespace HairSalonManagement.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public required DbSet<Salon> Salons { get; set; }
        public required DbSet<Employee> Employees { get; set; }
        public required DbSet<EmployeeService> EmployeeServices { get; set; }
        public required DbSet<Appointment> Appointments { get; set; }
        public required DbSet<Service> Services { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EmployeeService>()
                .HasKey(es => new { es.EmployeeId, es.ServiceId });

            modelBuilder.Entity<EmployeeService>()
                .HasOne(es => es.Employee)
                .WithMany(e => e.EmployeeServices)
                .HasForeignKey(es => es.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EmployeeService>()
                .HasOne(es => es.Service)
                .WithMany(s => s.EmployeeServices)
                .HasForeignKey(es => es.ServiceId)
                .OnDelete(DeleteBehavior.Restrict); // Burada kısıtlamayı belirtiyoruz

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Service)
                .WithMany()
                .HasForeignKey(a => a.ServiceId)
                .OnDelete(DeleteBehavior.Restrict); // Burada kısıtlamayı belirtiyoruz

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Salon)
                .WithMany()
                .HasForeignKey(a => a.SalonId)
                .OnDelete(DeleteBehavior.Restrict); // Burada kısıtlamayı belirtiyoruz

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Employee)
                .WithMany()
                .HasForeignKey(a => a.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict); // Burada kısıtlamayı belirtiyoruz
        }
    }
}