using Elvi.Az.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Elvi.Az.DAL
{
    public class AppDbContext:IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {

        }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Serv> Servs { get; set; }
        public DbSet<Pricing> Pricings { get; set; }
        public DbSet<Settings> Settings { get; set; }
       
    }
}
