using System.ComponentModel.DataAnnotations;

namespace HairSalonManagement.Models
{
public class Salon
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public TimeSpan OpeningTime { get; set; }
    public TimeSpan ClosingTime { get; set; }

    public ICollection<Service>? Services { get; set; }
}

}
