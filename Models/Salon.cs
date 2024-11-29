using System.ComponentModel.DataAnnotations;

namespace HairSalonManagement.Models
{
    public class Salon
    {
        public int Id { get; set; }
        [Required]
        public required string Name { get; set; }

        [Required]
        public required string WorkingHours { get; set; }
        public ICollection<Employee>? Employees { get; set; }
    }
}
