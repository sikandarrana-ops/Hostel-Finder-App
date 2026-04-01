using System.Text.Json;
using HostelFinderApp.Models;

namespace HostelFinderApp.Services
{
    public class UserService : IUserService
    {
        private readonly List<User> _users;
        private readonly string _filePath;
        private readonly object _lock = new object();
        private int _currentId = 1;

        public UserService()
        {
            var appDataPath = Path.Combine(Directory.GetCurrentDirectory(), "App_Data");
            if (!Directory.Exists(appDataPath))
            {
                Directory.CreateDirectory(appDataPath);
            }

            _filePath = Path.Combine(appDataPath, "users.json");

            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                _users = JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
                if (_users.Any())
                {
                    _currentId = _users.Max(u => u.Id) + 1;
                }
            }
            else
            {
                _users = new List<User>
                {
                    new User { Id = 1, Username = "admin", Password = "password", FullName = "Admin User" },
                    new User { Id = 2, Username = "test", Password = "password", FullName = "Test User" }
                };
                _currentId = 3;
                SaveData();
            }
        }

        private void SaveData()
        {
            var json = JsonSerializer.Serialize(_users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }

        public User? ValidateUser(string username, string password)
        {
            lock (_lock)
            {
                // Return a copy or just reference. Returning reference is fast but mutating it elsewhere could be bad.
                // Assuming callers don't maliciously mutate.
                return _users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) && u.Password == password);
            }
        }

        public User? GetUserById(int id)
        {
            lock (_lock)
            {
                return _users.FirstOrDefault(u => u.Id == id);
            }
        }

        public User? GetUserByUsername(string username)
        {
            lock (_lock)
            {
                return _users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
            }
        }

        public bool RegisterUser(User user)
        {
            lock (_lock)
            {
                if (_users.Any(u => u.Username.Equals(user.Username, StringComparison.OrdinalIgnoreCase)))
                {
                    return false;
                }

                user.Id = _currentId++;
                _users.Add(user);
                SaveData();
                return true;
            }
        }
    }
}
