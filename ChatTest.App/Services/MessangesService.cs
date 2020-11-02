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
        private static readonly object SyncRoot = new object();



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



        void ISeeder.Seed()
        {
            lock (SyncRoot)
            {
                var list = new List<Message>();

                if (Constants.Mock.UseMock)
                {
                    list.Add(new Message
                             {
                                 Id = Guid.NewGuid(),
                                 CreatedAt = DateTime.UtcNow,
                                 ConversationId = Constants.Mock.ConversationIds[0],
                                 Sender = Constants.Mock.UserNames[1],
                                 Text = "Heellloooo"
                             });
                    list.Add(new Message
                             {
                                 Id = Guid.NewGuid(),
                                 CreatedAt = DateTime.UtcNow.AddSeconds(1),
                                 ConversationId = Constants.Mock.ConversationIds[0],
                                 Sender = Constants.Mock.UserNames[1],
                                 Text = "Hi"
                             });
                    list.Add(new Message
                             {
                                 Id = Guid.NewGuid(),
                                 CreatedAt = DateTime.UtcNow.AddSeconds(2),
                                 ConversationId = Constants.Mock.ConversationIds[0],
                                 Sender = Constants.Mock.UserNames[0],
                                 Text = "What?"
                             });
                    list.Add(new Message
                             {
                                 Id = Guid.NewGuid(),
                                 CreatedAt = DateTime.UtcNow.AddSeconds(3),
                                 ConversationId = Constants.Mock.ConversationIds[0],
                                 Sender = Constants.Mock.UserNames[1],
                                 Text = "What are you doing tnit"
                             });
                    list.Add(new Message
                             {
                                 Id = Guid.NewGuid(),
                                 CreatedAt = DateTime.UtcNow.AddSeconds(4),
                                 ConversationId = Constants.Mock.ConversationIds[0],
                                 Sender = Constants.Mock.UserNames[1],
                                 Text = "tonight*"
                             });
                    list.Add(new Message
                             {
                                 Id = Guid.NewGuid(),
                                 CreatedAt = DateTime.UtcNow.AddSeconds(5),
                                 ConversationId = Constants.Mock.ConversationIds[0],
                                 Sender = Constants.Mock.UserNames[1],
                                 Text = "?"
                             });
                    list.Add(new Message
                             {
                                 Id = Guid.NewGuid(),
                                 CreatedAt = DateTime.UtcNow,
                                 ConversationId = Constants.Mock.ConversationIds[2],
                                 Sender = Constants.Mock.UserNames[0],
                                 Text = "Guys?"
                             });
                    list.Add(new Message
                             {
                                 Id = Guid.NewGuid(),
                                 CreatedAt = DateTime.UtcNow.AddSeconds(1),
                                 ConversationId = Constants.Mock.ConversationIds[2],
                                 Sender = Constants.Mock.UserNames[2],
                                 Text = "Yup?"
                             });
                    list.Add(new Message
                             {
                                 Id = Guid.NewGuid(),
                                 CreatedAt = DateTime.UtcNow.AddSeconds(2),
                                 ConversationId = Constants.Mock.ConversationIds[2],
                                 Sender = Constants.Mock.UserNames[1],
                                 Text = "Yea"
                             });
                }

                _cache.Set<IList<Message>>(MessangesCacheKey, list);
            }
        }
    }
}
