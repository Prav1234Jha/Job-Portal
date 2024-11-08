using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobPortal.Data.DataContext;
using JobPortal.Data.Entities;
using JobPortal.Data.ViewModel;
using JobPortal.Common;
using X.PagedList;
using Microsoft.Data.SqlClient;


namespace JobPortal.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/tblSystem")]
    [Authorize(Roles = "Admin")]
    public class tblSystemController : Controller
    {
        private readonly DataDbContext _context;

        public tblSystemController(DataDbContext context)
        {
            _context = context;
        }
        [Route("index")]
        [Route("")]
        [HttpGet]
        public async Task<IActionResult> Index(int? page, string sortOrder)
        {
            int pageSize = 5; //number of provinces per page
            ViewBag.SystemName = String.IsNullOrEmpty(sortOrder) ? "Name_Desc" : "";
            ViewBag.SystemDescription = sortOrder == "Description" ? "Description_Desc" : "Description";
            var systems = await _context.tblSystems.OrderBy(i => i.Name).ToListAsync();
            switch(sortOrder)
            {
                case "Name":
                    systems = await _context.tblSystems.OrderBy(i => i.Name).ToListAsync();
                    break;

                case "Name_Desc":
                    systems = await _context.tblSystems.OrderByDescending(i => i.Name).ToListAsync();
                    break;

                case "Description":
                    systems = await _context.tblSystems.OrderBy(i => i.Description).ToListAsync();
                    break;

                case "Description_Desc":
                    systems = await _context.tblSystems.OrderByDescending(i => i.Description).ToListAsync();
                    break;
                default:
                    systems = await _context.tblSystems.OrderBy(i => i.Name).ToListAsync();
                    break;
            }
            return View(systems.ToPagedList(page ?? 1, pageSize));
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
        public async Task<IActionResult> Create(IFormCollection frmcollection)
        {
            if (ModelState.IsValid)
            {
                tblSystem systems = new tblSystem();
                systems.Name = frmcollection["Name"];
                systems.Description = frmcollection["Description"];
                string strActivedata = frmcollection["IsActive"];
                if(strActivedata.Split(',').Count()>1 && strActivedata.Split(',')[0]=="true")
                {   
                    systems.IsActive = true;
                }
                else
                {
                    systems.IsActive = false;
                }
                _context.tblSystems.Add(systems);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [Route("update/{id}")]
        public IActionResult Update(int id)
        {
            var systems = _context.tblSystems.Where(u => u.SystemId == id).First();
            return View(systems);
        }

        [Route("update/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, IFormCollection frmcollection)
        {
            tblSystem systems = _context.tblSystems.Where(u => u.SystemId == id).First();

            systems.Name = frmcollection["Name"];
            systems.Description = frmcollection["Description"];
            string strActivedata = frmcollection["IsActive"];
            if (strActivedata.Split(',').Count() > 1 && strActivedata.Split(',')[0] == "true")
            {
                systems.IsActive = true;
            }
            else
            {
                systems.IsActive = false;
            }

            //systems.IsActive = true;
            _context.tblSystems.Update(systems);
            await _context.SaveChangesAsync();
            return Redirect("/admin/tblsystem");
        }

        [HttpGet("delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                tblSystem systems = _context.tblSystems.Where(d => d.SystemId == id).First();
                _context.tblSystems.Remove(systems);
                _context.SaveChanges();
                return Redirect("/admin/tblSystem");
            }
            catch (System.Exception)
            {
                return Redirect("/admin/tblSystem");
            }
        }

        [HttpPost("delete-selected")]
        public async Task<IActionResult> DeleteSelected(int[] listDelete)
        {
            foreach (int id in listDelete)
            {
                var systems = await _context.tblSystems.FindAsync(id);
                _context.tblSystems.Remove(systems);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
