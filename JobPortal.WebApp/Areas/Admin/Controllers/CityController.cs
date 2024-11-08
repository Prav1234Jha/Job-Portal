using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobPortal.Data.DataContext;
using JobPortal.Data.Entities;
using JobPortal.Data.ViewModel;
using JobPortal.Common;
using X.PagedList;

namespace JobPortal.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/city")]
    [Authorize(Roles = "Admin")]
    public class CityController : Controller
    {
        private readonly DataDbContext _context;

        public CityController(DataDbContext context)
        {
            _context = context;
        }
        [Route("index")]
        [Route("")]
        [HttpGet]
        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 5; //number of provinces per page

            var City = await _context.Cities.OrderBy(i => i.Id).ToListAsync();
            return View(City.ToPagedList(page ?? 1, pageSize));
        }
        [HttpGet]
        [Route("create")]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [Route("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCityViewModel model)
        {
            if (ModelState.IsValid)
            {
                City objcity = new City()
                {
                    Name = model.Name,
                    Slug = TextHelper.ToUnsignString(model.Name).ToLower(),
                    CategoryId=4,
                    Disable=false,
                    Popular=0
                };
                _context.Cities.Add(objcity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        
        [Route("update/{id}")]
        public IActionResult Update(int id)
        {
            var city = _context.Cities.Where(u => u.Id == id).First();
            return View(city);
        }

        [Route("update/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, UpdateCityModelView model)
        {
            City city = _context.Cities.Where(u => u.Id == id).First();
            city.Name = model.Name;
            city.Slug = TextHelper.ToUnsignString(city.Name).ToLower();
            _context.Cities.Update(city);
            await _context.SaveChangesAsync();
            return Redirect("/admin/city");
        }

        [HttpGet("delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                City city = _context.Cities.Where(d => d.Id == id).First();
                _context.Cities.Remove(city);
                _context.SaveChanges();
                return Redirect("/admin/city");
            }
            catch (System.Exception)
            {
                return Redirect("/admin/city");
            }
        }

        [HttpPost("delete-selected")]
        public async Task<IActionResult> DeleteSelected(int[] listDelete)
        {
            foreach (int id in listDelete)
            {
                var city = await _context.Cities.FindAsync(id);
                _context.Cities.Remove(city);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
