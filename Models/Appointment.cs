using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HairSalonManagement.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        [Required]
        public required string Service { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }
        public int Duration { get; set; }

        [Required]
        public decimal Price { get; set; }
        public bool IsConfirmed { get; set; }

        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
    }
}
