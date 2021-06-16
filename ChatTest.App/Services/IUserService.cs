using System.Collections.Generic;
using ChatTest.App.Models;

namespace ChatTest.App.Services
{
    public interface IUserService
    {
        User GetUser(string userName);
        User GetUserByToken(string userToken);
        User GetUserByConnection(string connectionId);
        IEnumerable<User> GetAll();
        bool IsValid(User user);
    }
}