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
    [Route("admin/tblTree")]
    [Authorize(Roles = "Admin")]
    public class tblTreeController : Controller
    {
        private readonly DataDbContext _context;
        public tblTreeController(DataDbContext context)
        {
            _context = context;
        }
        [Route("index")]
        [Route("")]
        [HttpGet]
        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 5; //number of provinces per page
            var trees = await _context.tblTrees.Include(x => x.system)
                .Include(x => x.module)
                .OrderBy(M => M.TreeId).ToListAsync();
            return View(trees.ToPagedList(page ?? 1, pageSize));
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
                tblTree trees = new tblTree();
                trees.Name = frmcollection["Name"];
                trees.Description = frmcollection["Description"];
                trees.NodeUrl = frmcollection["NodeUrl"];

                trees.SystemId = Convert.ToInt32(frmcollection["SystemId"]);

                trees.ModuleId = Convert.ToInt32(frmcollection["ModuleId"]);

                string strActivedata = frmcollection["IsActive"];
                if (strActivedata.Split(',').Count() > 1 && strActivedata.Split(',')[0] == "true")
                {
                    trees.IsActive = true;
                }
                else
                {
                    trees.IsActive = false;
                }
                _context.tblTrees.Add(trees);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [Route("update/{id}")]
        public IActionResult Update(int id)
        {
            var tree = _context.tblTrees.Where(u => u.TreeId == id).First();
            ViewBag.SystemData = new SelectList(_context.tblSystems, "SystemId", "Name");
            ViewBag.ModuleData = new SelectList(_context.tblModules, "ModuleId", "Name");
            return View(tree);
        }

        [Route("update/{id}")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, IFormCollection frmcollection)
        {
            tblTree tree = _context.tblTrees.Where(u => u.TreeId == id).First();

            tree.Name = frmcollection["Name"];
            tree.Description = frmcollection["Description"];
            tree.NodeUrl = frmcollection["NodeUrl"];

            tree.SystemId = Convert.ToInt32(frmcollection["SystemId"]);

            tree.ModuleId = Convert.ToInt32(frmcollection["ModuleId"]);
            string strActivedata = frmcollection["IsActive"];
            if (strActivedata.Split(',').Count() > 1 && strActivedata.Split(',')[0] == "true")
            {
                tree.IsActive = true;
            }
            else
            {
                tree.IsActive = false;
            }

            //function.IsActive = true;
            _context.tblTrees.Update(tree);
            await _context.SaveChangesAsync();
            return Redirect("/admin/tblTrees");
        }

        [HttpGet("delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                tblTree trees = _context.tblTrees.Where(d => d.TreeId == id).First();
                _context.tblTrees.Remove(trees);
                _context.SaveChanges();
                return Redirect("/admin/tblTree");
            }
            catch (Exception)
            {
                return Redirect("/admin/tblTree");
            }
        }

        [HttpPost("delete-selected")]
        public async Task<IActionResult> DeleteSelected(int[] listDelete)
        {
            foreach (int id in listDelete)
            {
                var tree = await _context.tblTrees.FindAsync(id);
                _context.tblTrees.Remove(tree);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}

