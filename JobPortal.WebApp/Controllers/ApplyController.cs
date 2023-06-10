﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobPortal.Data.DataContext;
using JobPortal.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using JobPortal.Data.ViewModel;
using JobPortal.Common;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Principal;
using System.Net.NetworkInformation;

namespace JobPortal.WebApp.Controllers
{
    [Route("apply")]
    public class ApplyController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly DataDbContext _context;

        public ApplyController(UserManager<AppUser> userManager, DataDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [Route("{id}")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ListApplies(Guid id)
        {
            var CVs = await (from cv in _context.CVs
                             orderby cv.Id descending
                             select new ListCVsViewModel()
                             {
                                 CVId = cv.Id,
                                 Certificate = cv.Certificate,
                                 Major = cv.Major,
                                 ApplyDate = cv.ApplyDate,
                                 GraduatedAt = cv.GraduatedAt,
                                 GPA = cv.GPA,
                                 Description = cv.Description,
                                 Introduce = cv.Introduce,
                                 UserId = cv.AppUserId,
                                 CVStatus = cv.Status,
                                 JobName = cv.Job.Name,
                                 EmployerLogo = cv.Job.AppUser.UrlAvatar
                             }).Where(u => u.UserId == id).ToListAsync();
            return View(CVs);
        }

        [Route("{slug}/{id}")]
        public async Task<IActionResult> Apply(string slug, Guid id)
        {
            var job = await _context.Jobs.Where(j => j.Slug == slug).FirstOrDefaultAsync();
            var user = await _context.AppUsers.Where(u => u.Id == id).FirstOrDefaultAsync();
            var cv = await _context.CVs.Where(x => x.AppUserId == id && x.JobId == job.Id).FirstOrDefaultAsync();
            // check role
            if (!User.IsInRole("User"))
            {
                return RedirectToAction(nameof(AccessDenied));
            }
            // check cv đã tồn tại
            else if (cv != null)
            {
                return RedirectToAction(nameof(Waiting));
            }
            else
            {
                return View();
            }
        }

        [Route("{slug}/{id}")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Apply(string slug, Guid id, CreateCVViewModel model)
        {
            var job = await _context.Jobs.Where(j => j.Slug == slug).FirstAsync();

            if (ModelState.IsValid)
            {
                string POST_IMAGE_PATH = "images/cvs/";

                if (model.UrlImage != null)
                {
                    var image = UploadImage.UploadImageFile(model.UrlImage, POST_IMAGE_PATH);

                    CV cv = new CV()
                    {
                        ApplyDate = DateTime.Now,
                        UrlImage = image,
                        Certificate = model.Certificate,
                        Major = model.Major,
                        GraduatedAt = model.GraduatedAt,
                        GPA = model.GPA,
                        Description = model.Description,
                        Introduce = model.Introduce,
                        AppUserId = id,
                        JobId = job.Id,
                        Status = 1 //waiting
                    };
                    _context.CVs.Add(cv);
                    await _context.SaveChangesAsync();
                    return Redirect("/apply/" + id);
                }
            }
            return RedirectToAction(nameof(Fail));
        }

        [Route("access-denied")]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [Route("fail")]
        public IActionResult Fail()
        {
            return View();
        }

        [Route("waiting")]
        public IActionResult Waiting()
        {
            return View();
        }
    }
}