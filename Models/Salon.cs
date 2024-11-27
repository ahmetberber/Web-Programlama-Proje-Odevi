namespace HairSalonManagement.Models
{
    public class Salon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string WorkingHours { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}
