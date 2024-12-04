using System.ComponentModel.DataAnnotations;

namespace HairSalonManagement.Models
{
    public class Service
    {
        public int Id { get; set; }
        [Required]
        public required string Name { get; set; }
        [Required]
        public required decimal Price { get; set; }
        [Required]
        public required int Duration { get; set; }
        [Required]
        public required int SalonId { get; set; }
        public Salon Salon { get; set; }

        public ICollection<EmployeeService>? EmployeeServices { get; set; }
    }
}
