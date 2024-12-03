using System.ComponentModel.DataAnnotations;

namespace HairSalonManagement.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Çalışan adı gereklidir.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Uzmanlık alanı gereklidir.")]
        public string Specialization { get; set; } // Örneğin: Saç Kesimi, Saç Boyama

        [Required(ErrorMessage = "Çalışma başlangıç saati gereklidir.")]
        public TimeSpan StartTime { get; set; } // Çalışma başlangıç saati

        [Required(ErrorMessage = "Çalışma bitiş saati gereklidir.")]
        public TimeSpan EndTime { get; set; } // Çalışma bitiş saati

        public ICollection<Service> Services { get; set; } // Çalışanın yapabildiği işlemler
    }
}
