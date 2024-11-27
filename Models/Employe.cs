namespace HairSalonManagement.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Expertise { get; set; }
        public bool IsAvailable { get; set; }
        public int SalonId { get; set; }
        public Salon Salon { get; set; }
    }
}
