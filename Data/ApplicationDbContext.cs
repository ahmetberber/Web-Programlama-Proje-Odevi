using Microsoft.EntityFrameworkCore;
using HairSalonManagement.Models;

namespace HairSalonManagement.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public required DbSet<Salon> Salons { get; set; }
        public required DbSet<Employee> Employees { get; set; }
        public required DbSet<Appointment> Appointments { get; set; }
    }
}
