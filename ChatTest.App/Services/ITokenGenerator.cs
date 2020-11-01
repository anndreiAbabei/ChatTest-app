namespace ChatTest.App.Services
{
    public interface ITokenGenerator
    {
        string Generate(string user, string client);
    }
}