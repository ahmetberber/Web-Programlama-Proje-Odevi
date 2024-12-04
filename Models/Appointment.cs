using System;
using System.ComponentModel.DataAnnotations;

namespace HairSalonManagement.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; } // Randevu tarihi ve saati

        [Required]
        public int Duration { get; set; } // Randevu süresi (dakika)

        public int SalonId { get; set; } // Randevunun yapıldığı salon
        public required Salon Salon { get; set; } // Salon ile ilişki

        [Required]
        public int ServiceId { get; set; } // Alınacak hizmet
        public required Service Service { get; set; } // Hizmet ile ilişki

        [Required]
        public int EmployeeId { get; set; } // Hizmeti gerçekleştiren çalışan
        public required Employee Employee { get; set; } // Çalışan ile ilişki
    }
}
