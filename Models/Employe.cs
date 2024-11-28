using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HairSalonManagement.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        public string Expertise { get; set; }

        public bool IsAvailable { get; set; } = true;

        [ForeignKey("Salon")]
        public int SalonId { get; set; }
        public Salon Salon { get; set; }
    }
}
