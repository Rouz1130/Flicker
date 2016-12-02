using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EpicPhotoShare.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace EpicPhotoShare.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHostingEnvironment _environment;

        public UserController (UserManager<ApplicationUser> userManager, ApplicationDbContext db, IHostingEnvironment environment)
        {
            _userManager = userManager;
            _db = db;
            _environment = environment;
        }
        public async Task<IActionResult> Index()
        {
            var id = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(id);
            return View(_db.Images.Where(x => x.User.Id == currentUser.Id));
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ICollection<IFormFile> files)
        {
            var id = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(id);
            var uploads = Path.Combine(_environment.WebRootPath, "uploads");
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                        Image image = new Image();
                        image.User = currentUser;
                        image.Source = Path.Combine("/Uploads/", file.FileName);
                        _db.Images.Add(image);
                        _db.SaveChanges();
                    }
                }
            }
      
            return RedirectToAction("Index");
        }
    }
}
