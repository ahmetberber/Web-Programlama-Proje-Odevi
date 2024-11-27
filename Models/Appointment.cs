namespace HairSalonManagement.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public string Service { get; set; }
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
