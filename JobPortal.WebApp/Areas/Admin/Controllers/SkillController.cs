﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JobPortal.Data.DataContext;
using JobPortal.Data.Entities;
using JobPortal.Data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using JobPortal.Common;

namespace JobPortal.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/skill")]
    [Authorize(Roles = "Admin")]
    public class SkillController : Controller
    {
        private readonly DataDbContext _context;

        public SkillController(DataDbContext context)
        {
            _context = context;
        }

        [Route("index")]
        [Route("")]
        public async Task<IActionResult> Index()
        {
            var skill = await _context.Skills.OrderByDescending(i => i.Id).ToListAsync();
            return View(skill);
        }

        [Route("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateSkillViewModel model)
        {
            if (ModelState.IsValid)
            {
                string POST_IMAGE_PATH = "images/skills/";

                if (model.Logo != null)
                {
                    var logo = UploadImage.UploadImageFile(model.Logo, POST_IMAGE_PATH);

                    Skill skill = new Skill()
                    {
                        Name = model.Name,
                        Slug = TextHelper.ToUnsignString(model.Name).ToLower(),
                        Logo = logo
                    };
                    _context.Skills.Add(skill);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        [Route("update/{id}")]
        public IActionResult Update(int id)
        {
            var skill = _context.Skills.Where(u => u.Id == id).First();
            return View(skill);
        }

        [Route("update/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, UpdateSkillViewModel model)
        {
            Skill skill = _context.Skills.Where(u => u.Id == id).First();
            skill.Name = model.Name;
            skill.Slug = TextHelper.ToUnsignString(model.Name).ToLower();
            _context.Skills.Update(skill);
            await _context.SaveChangesAsync();
            return Redirect("/admin/skill");
        }

        [Route("update-image/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateImage(int id, IFormFile Logo)
        {
            string POST_IMAGE_PATH = "images/skills/";

            if (Logo != null)
            {
                Skill skill = _context.Skills.Where(u => u.Id == id).First();
                string oldLogoImage = skill.Logo;
                var newLogoImage = UploadImage.UploadImageFile(Logo, POST_IMAGE_PATH);
                skill.Logo = newLogoImage;
                _context.Update(skill);
                await _context.SaveChangesAsync();

                if (!string.IsNullOrEmpty(oldLogoImage))
                {
                    string oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "skills", oldLogoImage);
                    DeleteImage.DeleteImageFile(oldImagePath);
                }

                return Redirect("/admin/skill/update/" + id);
            }
            return Redirect("/admin/skill/update/" + id);
        }

        [HttpGet("delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                Skill skill = _context.Skills.Where(p => p.Id == id).First();
                if (skill != null)
                {
                    string imageName = skill.Logo;
                    _context.Skills.Remove(skill);
                    _context.SaveChanges();
                    if (!string.IsNullOrEmpty(imageName))
                    {
                        string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "skills", imageName);
                        DeleteImage.DeleteImageFile(imagePath);
                    }
                }
                return Redirect("/admin/skill");
            }
            catch (System.Exception)
            {
                return Redirect("/admin/skill");
            }
        }

        [HttpPost("delete-selected")]
        public async Task<IActionResult> DeleteSelected(int[] listDelete)
        {
            foreach (int id in listDelete)
            {
                var skill = await _context.Skills.FindAsync(id);
                _context.Skills.Remove(skill);

                if (skill != null)
                {
                    string imageName = skill.Logo;

                    _context.Skills.Remove(skill);
                    _context.SaveChanges();

                    if (!string.IsNullOrEmpty(imageName))
                    {
                        string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "skills", imageName);
                        DeleteImage.DeleteImageFile(imagePath);
                    }
                }
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [Route("fail")]
        public IActionResult Fail()
        {
            return View();
        }
    }
}