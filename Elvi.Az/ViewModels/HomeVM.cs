using Elvi.Az.Models;

namespace Elvi.Az.ViewModels;

public class HomeVM
{
    public List<Pricing> Pricings { get; set; }
    public List<Serv> Servs { get; set; }
    public Settings Settings { get; set; }
    public List<Team> Teams { get; set; }
}
