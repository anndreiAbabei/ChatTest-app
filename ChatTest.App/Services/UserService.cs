using System;
using System.Collections.Generic;
using System.Linq;
using ChatTest.App.Models;
using Microsoft.Extensions.Caching.Memory;

namespace ChatTest.App.Services
{
    public class UserService : IUserService
    {
        private readonly IMemoryCache _cache;
        private readonly ITokenGenerator _tokenGenerator;
        private const string UsersCacheKey = "users";


        public UserService(IMemoryCache cache, ITokenGenerator tokenGenerator)
        {
            _cache = cache;
            _tokenGenerator = tokenGenerator;
        }

        public User GetUser(string userName)
        {
            return _cache.Get<IList<User>>(UsersCacheKey)?
               .FirstOrDefault(u => u.Name == userName);
        }



        public User GetUserByToken(string userToken)
        {
            User user = _cache.Get<IList<User>>(UsersCacheKey)?
               .FirstOrDefault(u => u.Token == userToken);

            if (user != null)
                return user;

            user = new User
                   {
                       Name = _tokenGenerator.GetName(userToken),
                       ConnectionId = _tokenGenerator.GetConnectionId(userToken),
                       Token = userToken
                   };

            return user;
        }



        public User GetUserByConnection(string connectionId)
        {
            return _cache.Get<IList<User>>(UsersCacheKey)?
               .FirstOrDefault(u => u.ConnectionId == connectionId);
        }



        public IEnumerable<User> GetAll()
        {
            return _cache.Get<IList<User>>(UsersCacheKey);
        }



        public bool IsValid(User user) => _tokenGenerator.IsValid(user.Token);
    }
}
