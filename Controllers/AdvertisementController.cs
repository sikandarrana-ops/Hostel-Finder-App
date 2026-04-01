using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using HostelFinderApp.Models;
using HostelFinderApp.Services;
using Microsoft.AspNetCore.Hosting;

namespace HostelFinderApp.Controllers
{
    [Authorize]
    public class AdvertisementController : Controller
    {
        private readonly IAdvertisementService _advertisementService;
        private readonly IWebHostEnvironment _env;

        public AdvertisementController(IAdvertisementService advertisementService, IWebHostEnvironment env)
        {
            _advertisementService = advertisementService;
            _env = env;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var ads = userId != null ? _advertisementService.GetByUserId(userId) : new List<Advertisement>();
            return View(ads);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Advertisement());
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Advertisement model, List<IFormFile>? imageFiles)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var userName = User.FindFirstValue(ClaimTypes.Name) ?? "Unknown";

                if (userId != null)
                {
                    if (imageFiles != null && imageFiles.Count > 0)
                    {
                        var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
                        Directory.CreateDirectory(uploadsFolder);
                        foreach (var file in imageFiles)
                        {
                            if (file.Length > 0)
                            {
                                var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                                using (var fileStream = new FileStream(filePath, FileMode.Create))
                                {
                                    await file.CopyToAsync(fileStream);
                                }
                                model.ImageUrls.Add("/uploads/" + uniqueFileName);
                            }
                        }
                        
                        if (model.ImageUrls.Any())
                        {
                            model.ImageUrl = model.ImageUrls.First(); // Standard fallback thumbnail
                        }
                    }

                    model.UserId = userId;
                    model.UserName = userName;
                    _advertisementService.Add(model);
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            var ad = _advertisementService.GetById(id);
            if (ad == null) return NotFound();
            return View(ad);
        }
    }
}
