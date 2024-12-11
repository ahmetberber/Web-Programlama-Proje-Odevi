using System.ComponentModel.DataAnnotations;

namespace HairSalonManagement.Models
{
    public enum SalonType
    {
        ErkekKuaforu,
        GuzellikSalonu
    }

    public class Salon
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Address { get; set; }
        public TimeSpan OpeningTime { get; set; }
        public TimeSpan ClosingTime { get; set; }
        public SalonType Type { get; set; }
        public ICollection<Service>? Services { get; set; }
        public ICollection<Employee>? Employees { get; set; }
    }

}
