using System.Text.Json;
using HostelFinderApp.Models;

namespace HostelFinderApp.Services
{
    public class AdvertisementService : IAdvertisementService
    {
        private readonly List<Advertisement> _advertisements;
        private readonly string _filePath;
        private readonly object _lock = new object();
        private int _currentId = 1;

        public AdvertisementService()
        {
            var appDataPath = Path.Combine(Directory.GetCurrentDirectory(), "App_Data");
            if (!Directory.Exists(appDataPath))
            {
                Directory.CreateDirectory(appDataPath);
            }

            _filePath = Path.Combine(appDataPath, "ads.json");

            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                _advertisements = JsonSerializer.Deserialize<List<Advertisement>>(json) ?? new List<Advertisement>();
                if (_advertisements.Any())
                {
                    _currentId = _advertisements.Max(a => a.Id) + 1;
                }
            }
            else
            {
                _advertisements = new List<Advertisement>
                {
                    new Advertisement
                    {
                        Id = _currentId++,
                        Title = "Cozy Room near University",
                        Description = "A nice room for a student. Shared kitchen.",
                        Location = "Downtown",
                        Price = 500,
                        UserId = "1",
                        UserName = "admin",
                        DatePosted = DateTime.Now.AddDays(-2)
                    },
                    new Advertisement
                    {
                        Id = _currentId++,
                        Title = "Heated Room near University",
                        Description = "A nice room for a student. Shared kitchen.",
                        Location = "Downtown",
                        Price = 500,
                        UserId = "1",
                        UserName = "admin",
                        DatePosted = DateTime.Now.AddDays(-2)
                    },
                    new Advertisement
                    {
                        Id = _currentId++,
                        Title = "Cold Room near University",
                        Description = "A nice room for a student. Shared kitchen.",
                        Location = "Downtown",
                        Price = 500,
                        UserId = "1",
                        UserName = "admin",
                        DatePosted = DateTime.Now.AddDays(-2)
                    },
                    new Advertisement
                    {
                        Id = _currentId++,
                        Title = "Cozy Room near University",
                        Description = "A nice room for a student. Shared kitchen.",
                        Location = "Downtown",
                        Price = 500,
                        UserId = "1",
                        UserName = "admin",
                        DatePosted = DateTime.Now.AddDays(-2)
                    },
                    new Advertisement
                    {
                        Id = _currentId++,
                        Title = "Perfect Room near University",
                        Description = "A nice room for a student. Shared kitchen.",
                        Location = "Downtown",
                        Price = 500,
                        UserId = "1",
                        UserName = "admin",
                        DatePosted = DateTime.Now.AddDays(-2)
                    }
                };
                SaveData();
            }
        }

        private void SaveData()
        {
            var json = JsonSerializer.Serialize(_advertisements, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }

        public IEnumerable<Advertisement> GetAll(int? limit = null)
        {
            lock (_lock)
            {
                var query = _advertisements.OrderByDescending(a => a.DatePosted);
                if (limit.HasValue)
                {
                    return query.Take(limit.Value).ToList();
                }
                return query.ToList();
            }
        }

        public Advertisement? GetById(int id)
        {
            lock (_lock)
            {
                return _advertisements.FirstOrDefault(a => a.Id == id);
            }
        }

        public void Add(Advertisement advertisement)
        {
            lock (_lock)
            {
                advertisement.Id = _currentId++;
                advertisement.DatePosted = DateTime.Now;
                _advertisements.Add(advertisement);
                SaveData();
            }
        }

        public IEnumerable<Advertisement> GetByUserId(string userId)
        {
            lock (_lock)
            {
                return _advertisements.Where(a => a.UserId == userId).OrderByDescending(a => a.DatePosted).ToList();
            }
        }

        public IEnumerable<Advertisement> Search(string? city, decimal? minPrice, decimal? maxPrice)
        {
            lock (_lock)
            {
                var query = _advertisements.AsEnumerable();

                if (!string.IsNullOrWhiteSpace(city))
                {
                    query = query.Where(a => a.Location.Contains(city, StringComparison.OrdinalIgnoreCase));
                }

                if (minPrice.HasValue)
                {
                    query = query.Where(a => a.Price >= minPrice.Value);
                }

                if (maxPrice.HasValue)
                {
                    query = query.Where(a => a.Price <= maxPrice.Value);
                }

                return query.OrderByDescending(a => a.DatePosted).ToList();
            }
        }
    }
}
