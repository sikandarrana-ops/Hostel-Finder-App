using System.Diagnostics;
using HostelFinderApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HostelFinderApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HostelFinderApp.Services.IAdvertisementService _adService;

        public HomeController(ILogger<HomeController> logger, HostelFinderApp.Services.IAdvertisementService adService)
        {
            _logger = logger;
            _adService = adService;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            
            var vm = new HomeViewModel
            {
                MyAds = userId != null ? _adService.GetByUserId(userId) : new List<Advertisement>()
            };

            return View(vm);
        }

        public IActionResult Search(string? searchCity, decimal? minPrice, decimal? maxPrice)
        {
            var vm = new HomeViewModel
            {
                SearchCity = searchCity,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                SearchResults = _adService.Search(searchCity, minPrice, maxPrice)
            };

            return View(vm);
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
