using System;
using System.ComponentModel.DataAnnotations;

namespace HairSalonManagement.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int SalonId { get; set; }
        public Salon? Salon { get; set; }

        [Required]
        public int ServiceId { get; set; }
        public Service? Service { get; set; }

        [Required]
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }

        public DateTime CreatedAt { get; set; }
        public required string CreatedBy { get; set; }
    }
}