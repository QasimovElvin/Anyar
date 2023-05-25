using Elvi.Az.DAL;
using Elvi.Az.Models;
using Elvi.Az.Utilities.Extensions;
using Elvi.Az.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Elvi.Az.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class TeamController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public TeamController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index(int page = 1,int take=3)
        {
            var service= _context.Teams.Skip((page - 1) * take).Take(take).ToList();
            PaginateVM<Team> paginateVM = new PaginateVM<Team>()
            {
                Items = service,
                PageCount = PageCount(take),
                CurrentPage = page

            };
            return View(paginateVM);
        }
        private int PageCount(int take)
        {
            var pagecount = _context.Teams.Count();
            return (int)Math.Ceiling((decimal)pagecount / take);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Team team)
        {
            if (!ModelState.IsValid) return View();
            if (team.ImageFile == null)
            {
                ModelState.AddModelError("", "Image is null");
                return View();
            }
            if (!team.ImageFile.CheckType("image/"))
            {
                ModelState.AddModelError("", "Image Type invalid");
                return View();
            }
            if (team.ImageFile.CheckSize(500))
            {
                ModelState.AddModelError("", "Image size Wrong");
                return View();
            }
            team.Image =await team.ImageFile.SaveFileAsync(_env.WebRootPath, "assets/img/team");
            Team user=new Team()
            {
                Name = team.Name,
                Profession = team.Profession,
                Description = team.Description,
                Image=team.Image,
            };
            await _context.AddAsync(team);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int id)
        {
            Team? team = _context.Teams.FirstOrDefault(x=>x.Id==id);
            return View(team);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Team team)
        {
            Team? exist = _context.Teams.FirstOrDefault(x => x.Id == team.Id);
            if (exist == null)
            {
                ModelState.AddModelError("", "Team us null");
                return View();
            }
            if (team.ImageFile != null)
            {
                if (!team.ImageFile.CheckType("Image/"))
                {
                    ModelState.AddModelError("", "Image Type invalid");
                    return View();
                }
                if (team.ImageFile.CheckSize(500))
                {
                    ModelState.AddModelError("", "Image size Wrong");
                    return View();
                }
                string path =Path.Combine(_env.WebRootPath, "assets/img/team",exist.Image);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                exist.Image = await team.ImageFile.SaveFileAsync(_env.WebRootPath, "assets/img/team");
            }
            exist.Name=team.Name;
            exist.Description=team.Description;
            exist.Profession = team.Profession;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            Team? team = _context.Teams.FirstOrDefault(x => x.Id == id);
            string path = Path.Combine(_env.WebRootPath, "assets/img/team",team.Image);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
             _context.Remove(team);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
    }
}
