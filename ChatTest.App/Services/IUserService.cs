using System.Collections.Generic;
using ChatTest.App.Models;

namespace ChatTest.App.Services
{
    public interface IUserService : ISeeder
    {
        User GetUser(string userName);
        User GetUserByToken(string userToken);
        User GetUserByConnection(string connectionId);
        IEnumerable<User> GetAll();
        void Register(User user);
        void Remove(string userName);
        bool IsValid(User user);
    }
}