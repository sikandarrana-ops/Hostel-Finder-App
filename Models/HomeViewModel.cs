namespace HostelFinderApp.Models
{
    public class HomeViewModel
    {
        public IEnumerable<Advertisement> MyAds { get; set; } = new List<Advertisement>();
        public IEnumerable<Advertisement> SearchResults { get; set; } = new List<Advertisement>();

        // Search Parameters
        public string? SearchCity { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool IsSearchActive => !string.IsNullOrEmpty(SearchCity) || MinPrice.HasValue || MaxPrice.HasValue;
    }
}
