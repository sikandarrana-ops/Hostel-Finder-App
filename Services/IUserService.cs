using HostelFinderApp.Models;

namespace HostelFinderApp.Services
{
    public interface IUserService
    {
        User? ValidateUser(string username, string password);
        User? GetUserById(int id);
        User? GetUserByUsername(string username);
        bool RegisterUser(User user);
    }
}
