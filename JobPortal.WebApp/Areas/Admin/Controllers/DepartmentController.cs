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

namespace JobPortal.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/Department")]
    [Authorize(Roles = "Admin")]
    public class DepartmentController : Controller
    {

        private readonly DataDbContext _context;

        public DepartmentController(DataDbContext context)
        {
            _context = context;
        }
        [Route("index")]
        [Route("")]
        [HttpGet]
        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 5; //number of provinces per page

            var Department = await _context.Departments.OrderBy(i => i.Id).ToListAsync();
            return View(Department.ToPagedList(page ?? 1, pageSize));
        }
        [HttpGet]
        [Route("create")]
        public IActionResult Create()
        {
        //    List<Category> lstcategory = _context.Categories.ToList();
        //    SelectList lstCategory = new SelectList(lstcategory, "ID", "Name", "1");
        //    ViewBag.Category = lstCategory;
        //    return View(lstCategory);
            return View();
        }


        [HttpPost]
        [Route("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDepartmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                Department objdepart = new Department()
                {
                    DepartmentName = model.DepartmentName,
                    Slug = TextHelper.ToUnsignString(model.DepartmentName).ToLower(),
                    CategoryId = 4,
                    Disable = false,
                    Popular = 0
                };
                _context.Departments.Add(objdepart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }


        [Route("update/{id}")]
        public IActionResult Update(int id)
        {
            var depart = _context.Departments.Where(u => u.Id == id).First();
            return View(depart);
        }

        [Route("update/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, UpdateDepartmentViewModel model)
        {
            Department department = _context.Departments.Where(u => u.Id == id).First();
            department.DepartmentName = model.DepartmentName;
            department.Slug = TextHelper.ToUnsignString(department.DepartmentName).ToLower();
            _context.Departments.Update(department);
            await _context.SaveChangesAsync();
            return Redirect("/admin/department");
        }

        [HttpGet("delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                Department department = _context.Departments.Where(d => d.Id == id).First();
                _context.Departments.Remove(department);
                _context.SaveChanges();
                return Redirect("/admin/department");
            }
            catch (System.Exception)
            {
                return Redirect("/admin/department");
            }
        }

        [HttpPost("delete-selected")]
        public async Task<IActionResult> DeleteSelected(int[] listDelete)
        {
            foreach (int id in listDelete)
            {
                var depart = await _context.Departments.FindAsync(id);
                _context.Departments.Remove(depart);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}

