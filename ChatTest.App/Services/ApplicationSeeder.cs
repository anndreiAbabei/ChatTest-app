namespace ChatTest.App.Services
{
    public class ApplicationSeeder : ISeeder
    {
        private readonly IUserService _userService;
        private readonly IConversationService _conversationService;
        private readonly IMessangesService _messangesService;



        public ApplicationSeeder(IUserService userService, IConversationService conversationService, IMessangesService messangesService)
        {
            _userService = userService;
            _conversationService = conversationService;
            _messangesService = messangesService;
        }



        public void Seed()
        {
            _userService.Seed();
            _conversationService.Seed();
            _messangesService.Seed();
        }
    }
}
