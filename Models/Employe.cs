using System.ComponentModel.DataAnnotations;

namespace HairSalonManagement.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [Required]
        public required string Name { get; set; }
        [Required]
        public TimeSpan StartTime { get; set; }
        [Required]
        public TimeSpan EndTime { get; set; }
        [Required]
        public int SalonId { get; set; }
        public required ICollection<EmployeeService> EmployeeServices { get; set; }
    }
}
