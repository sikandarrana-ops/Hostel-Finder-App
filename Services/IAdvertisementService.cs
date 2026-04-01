using HostelFinderApp.Models;

namespace HostelFinderApp.Services
{
    public interface IAdvertisementService
    {
        IEnumerable<Advertisement> GetAll(int? limit = null);
        Advertisement? GetById(int id);
        void Add(Advertisement advertisement);
        IEnumerable<Advertisement> GetByUserId(string userId);
        IEnumerable<Advertisement> Search(string? city, decimal? minPrice, decimal? maxPrice);
    }
}
