using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobPortal.Data.DataContext;
using JobPortal.Data.Entities;
using JobPortal.Data.ViewModel;
using JobPortal.Common;
using X.PagedList;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JobPortal.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/tblModule")]
    [Authorize(Roles = "Admin")]
    public class tblModuleController : Controller
    {
        private readonly DataDbContext _context;
        public tblModuleController(DataDbContext context)
        {
            _context = context;
        }
        [Route("index")]
        [Route("")]
        [HttpGet]
        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 5; //number of provinces per page
            var module = await _context.tblModules.Include(X=>X.system)
                .OrderBy(i => i.ModuleId).ToListAsync();            
            return View(module.ToPagedList(page ?? 1, pageSize));
        }

        [HttpGet]
        [Route("create")]
        public IActionResult Create()
        {
            ViewBag.SystemData = new SelectList(_context.tblSystems, "SystemId", "Name");

            return View();
        }

        [HttpPost]
        [Route("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection frmcollection)
        {
            if (ModelState.IsValid)
            {
                tblModule module = new tblModule();
                module.Name = frmcollection["Name"];
                module.Description = frmcollection["Description"];

                module.SystemId = Convert.ToInt32(frmcollection["SystemId"]);

                module.ModuleUrl = frmcollection["ModuleUrl"];
                string strActivedata = frmcollection["IsActive"];
                if (strActivedata.Split(',').Count() > 1 && strActivedata.Split(',')[0] == "true")
                {
                    module.IsActive = true;
                }
                else
                {
                    module.IsActive = false;
                }
                _context.tblModules.Add(module);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [Route("update/{id}")]
        public IActionResult Update(int id)
        {
            var module = _context.tblModules.Where(u => u.ModuleId == id).First();
            ViewBag.SystemData = new SelectList(_context.tblSystems, "SystemId", "Name");

            return View(module);
        }

        [Route("update/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, IFormCollection frmcollection)
        {
            tblModule module = _context.tblModules.Where(u => u.ModuleId == id).First();

            module.Name = frmcollection["Name"];
            module.Description = frmcollection["Description"];
            string strActivedata = frmcollection["IsActive"];
            if (strActivedata.Split(',').Count() > 1 && strActivedata.Split(',')[0] == "true")
            {
                module.IsActive = true;
            }
            else
            {
                module.IsActive = false;
            }

            //module.IsActive = true;
            _context.tblModules.Update(module);
            await _context.SaveChangesAsync();
            return Redirect("/admin/tblModule");
        }

        [HttpGet("delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                tblModule module = _context.tblModules.Where(d => d.ModuleId == id).First();
                _context.tblModules.Remove(module);
                _context.SaveChanges();
                return Redirect("/admin/tblModule");
            }
            catch (Exception)
            {
                return Redirect("/admin/tblModule");
            }
        }

        [HttpPost("delete-selected")]
        public async Task<IActionResult> DeleteSelected(int[] listDelete)
        {
            foreach (int id in listDelete)
            {
                var module = await _context.tblModules.FindAsync(id);
                _context.tblModules.Remove(module);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
