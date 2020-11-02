using ChatTest.App.Models;

namespace ChatTest.App.Services
{
    public interface ITokenGenerator
    {
        string Generate(string user, CliendId client);
        bool IsValid(string token);
        string GetName(string token);
        string GetConnectionId(string token);
    }
}