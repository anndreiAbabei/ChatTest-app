using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChatTest.App.Hubs;
using ChatTest.App.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;

namespace ChatTest.App.Services
{
    public class MessangesService : IMessangesService
    {
        private readonly IMemoryCache _cache;
        private readonly IConversationService _conversationService;
        private readonly IUserService _userService;
        private readonly IHubContext<ChatHub> _hubContext;
        private const string MessangesCacheKey = "messages";



        public MessangesService(IMemoryCache cache, 
                                IConversationService conversationService, 
                                IUserService userService,
                                IHubContext<ChatHub> hubContext)
        {
            _cache = cache;
            _conversationService = conversationService;
            _userService = userService;
            _hubContext = hubContext;
        }

        public IEnumerable<MessageModel> GetMessages(Guid conversationId, string userName)
        {
            return GetMessages().Where(m => m.ConversationId == conversationId)
                                .OrderBy(m => m.CreatedAt)
                                .Select(m => new MessageModel
                                             {
                                                 Id = m.Id,
                                                 CreatedAt = m.CreatedAt,
                                                 Text = m.Text,
                                                 Sender = m.Sender,
                                                 ConversationId = m.ConversationId,
                                                 IsMine = m.Sender == userName
                                             });
        }



        public async ValueTask Create(string userName, Guid conversationId, string text, CancellationToken cancellationToken = default)
        {
            var message = new Message
                          {
                              Id = Guid.NewGuid(),
                              CreatedAt = DateTime.UtcNow,
                              Sender = userName,
                              ConversationId = conversationId,
                              Text = text
                          };

            Conversation conv = _conversationService.Get(conversationId, userName);

            if (conv != null)
            {
                conv.Text = text;
                conv.Reads.Clear();

                List<string> users = conv.Participants
                                         .Where(p => p != message.Sender)
                                         .Select(_userService.GetUser)
                                         .Where(u => u != null)
                                         .Select(u => u.ConnectionId)
                                         .ToList();

                if(users.Count > 0)
                    //Users(users)
                    await _hubContext.Clients.All.SendAsync(ChatHub.SendMessageMethod, message, cancellationToken)
                                     .ConfigureAwait(false);
            }

            GetMessages().Add(message);
        }



        private IList<Message> GetMessages()
        {
            return _cache.Get<IList<Message>>(MessangesCacheKey);
        }
    }
}
