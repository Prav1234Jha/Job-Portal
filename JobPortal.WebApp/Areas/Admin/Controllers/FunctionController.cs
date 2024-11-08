using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobPortal.Data.DataContext;
using JobPortal.Data.Entities;
using JobPortal.Data.ViewModel;
using JobPortal.Common;
using X.PagedList;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace JobPortal.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/tblFunction")]
    [Authorize(Roles = "Admin")]
    public class tblFunctionController : Controller
    {
        private readonly DataDbContext _context;
        public tblFunctionController(DataDbContext context)
        {
            _context = context;
        }
        [Route("index")]
        [Route("")]
        [HttpGet]
        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 5; //number of provinces per page
            var function = await _context.tblFunctions.Include(x=>x.system)
                .Include(x=>x.module)                
                .OrderBy(M => M.FunctionId).ToListAsync();           
            return View(function.ToPagedList(page ?? 1, pageSize));
        }
        [HttpGet]
        [Route("create")]
        public IActionResult Create()
        {
            ViewBag.SystemData = new SelectList(_context.tblSystems, "SystemId", "Name");
            ViewBag.ModuleData = new SelectList(_context.tblModules, "ModuleId", "Name");

            return View();
        }

        [HttpPost]
        [Route("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection frmcollection)
        {
            if (ModelState.IsValid)
            {
                tblFunction function = new tblFunction();
                function.Name = frmcollection["Name"];
                function.Description = frmcollection["Description"];

                function.SystemId = Convert.ToInt32(frmcollection["SystemId"]);

                function.ModuleId = Convert.ToInt32(frmcollection["ModuleId"]);

                string strActivedata = frmcollection["IsActive"];
                if (strActivedata.Split(',').Count() > 1 && strActivedata.Split(',')[0] == "true")
                {
                    function.IsActive = true;
                }
                else
                {
                    function.IsActive = false;
                }
                _context.tblFunctions.Add(function);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [Route("update/{id}")]
        public IActionResult Update(int id)
        {
            var module = _context.tblFunctions.Where(u => u.FunctionId == id).First();
            ViewBag.SystemData = new SelectList(_context.tblSystems, "SystemId", "Name");
            ViewBag.ModuleData = new SelectList(_context.tblModules, "ModuleId", "Name");
            return View(module);
        }

        [Route("update/{id}")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, IFormCollection frmcollection)
        {
            tblFunction function = _context.tblFunctions.Where(u => u.FunctionId == id).First();

            function.Name = frmcollection["Name"];
            function.Description = frmcollection["Description"];

            function.SystemId = Convert.ToInt32(frmcollection["SystemId"]);

            function.ModuleId = Convert.ToInt32(frmcollection["ModuleId"]);
            string strActivedata = frmcollection["IsActive"];
            if (strActivedata.Split(',').Count() > 1 && strActivedata.Split(',')[0] == "true")
            {
                function.IsActive = true;
            }
            else
            {
                function.IsActive = false;
            }

            //function.IsActive = true;
            _context.tblFunctions.Update(function);
            await _context.SaveChangesAsync();
            return Redirect("/admin/tblFunction");
        }
        [HttpGet("delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                tblFunction function = _context.tblFunctions.Where(d => d.FunctionId == id).First();
                _context.tblFunctions.Remove(function);
                _context.SaveChanges();
                return Redirect("/admin/tblFunctions");
            }
            catch (Exception)
            {
                return Redirect("/admin/tblFunctions");
            }
        }

        [HttpPost("delete-selected")]
        public async Task<IActionResult> DeleteSelected(int[] listDelete)
        {
            foreach (int id in listDelete)
            {
                var function = await _context.tblFunctions.FindAsync(id);
                _context.tblFunctions.Remove(function);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
