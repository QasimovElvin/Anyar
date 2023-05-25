using System.ComponentModel.DataAnnotations.Schema;

namespace Elvi.Az.Models;

public class Team
{
    public int Id { get; set; }
    public string? Image { get; set; }
    [NotMapped]
    public IFormFile ImageFile { get; set; } = null!;
    public string Name { get; set; }= null!;
    public string Profession { get; set; }= null!;
    public string Description { get; set; }= null!;
}
