using System.Collections.Generic;

namespace ChatTest.App.Services
{
    public interface IUserNameGenerator
    {
        string Generate();
        IEnumerable<string> All { get; }
    }
}