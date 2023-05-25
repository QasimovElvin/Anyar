using Elvi.Az.DAL;
using Elvi.Az.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Elvi.Az.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
           HomeVM homeVM = new HomeVM()
           {
               Pricings=_context.Pricings.Take(4).ToList(),
               Servs=_context.Servs.Take(4).ToList(),
               Teams=_context.Teams.Take(4).ToList(),
           };
            return View(homeVM);
        }

        

      
    }
}