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
    public class ConversationService : IConversationService
    {
        private readonly IMemoryCache _cache;
        private readonly IUserService _userService;
        private readonly IHubContext<ChatHub> _hubContext;
        private const string ConversationsCacheKey = "conversations";



        public ConversationService(IMemoryCache cache, IUserService userService, IHubContext<ChatHub> hubContext)
        {
            _cache = cache;
            _userService = userService;
            _hubContext = hubContext;
        }



        public IEnumerable<ConversationModel> GetUserConversations(string userName)
        {
            IList<Conversation> conversations = GetConversations();

            return conversations.Where(c => c.Participants.Contains(userName))
                                .Select(c => new ConversationModel
                                             {
                                                 Id = c.Id,
                                                 Name = c.Name,
                                                 Participants = c.Participants,
                                                 CreatedAt = c.CreatedAt,
                                                 Text = c.Text,
                                                 CreatedBy = c.CreatedBy,
                                                 Online = c.Online,
                                                 Reads = c.Reads,
                                                 Read = c.Reads.ContainsKey(userName)
                                             })
                                .Select(c => FillConversation(c, userName));
        }



        public bool Exists(IEnumerable<string> participants, string userName = null)
        {
            IList<Conversation> conversations = GetConversations();
            IList<string> testPart = participants as IList<string> ?? participants.ToList();

            return conversations.Any(c =>
            {
                IList<string> convParts = c.Participants as IList<string> ?? c.Participants.ToList();

                return convParts.Count == testPart.Count && 
                       convParts.All(testPart.Contains) &&
                       (string.IsNullOrEmpty(userName) || c.Name == userName);
            });
        }



        public Conversation Get(Guid conversationId, string userName)
        {
            return FillConversation(GetConversations().FirstOrDefault(c => c.Id == conversationId), 
                                    userName);
        }



        public async ValueTask<Conversation> Create(string name, 
                                                    IEnumerable<string> participants, 
                                                    string userName, 
                                                    CancellationToken cancellationToken = default)
        {
            var newConv = new Conversation
                          {
                                Id = Guid.NewGuid(),
                                CreatedAt = DateTime.UtcNow,
                                CreatedBy = userName,
                                Name = name,
                                Participants = participants as IList<string> ?? participants.ToList()
                          };

            GetConversations().Add(newConv);

            await _hubContext.Clients.All.SendAsync(ChatHub.SendMessageMethod, newConv, cancellationToken)
                                      .ConfigureAwait(false);

            return newConv;
        }



        public void MarkAsRead(Guid conversationId, string userName)
        {
            Get(conversationId, userName)?.Reads.TryAdd(userName, DateTime.UtcNow);
        }



        public async ValueTask Delete(Guid conversationId, CancellationToken cancellationToken = default)
        {
            IList<Conversation> conversations = GetConversations();
            Conversation conv = conversations.FirstOrDefault(c => c.Id == conversationId);

            if (conv != null)
            {
                conversations.Remove(conv);

                await _hubContext.Clients.All.SendAsync(ChatHub.DeleteConversationMethod, conv.Id, cancellationToken)
                                 .ConfigureAwait(false);
            }
        }



        private TConversation FillConversation<TConversation>(TConversation conversation, string user)
            where TConversation : Conversation
        {
            if (string.IsNullOrEmpty(conversation.Name))
                conversation.Name = string.Join(", ", conversation.Participants.Where(p => p != conversation.CreatedBy));

            var participants = conversation.Participants as IList<string> ?? conversation.Participants.ToList();

            if(participants.Count == 2)
                conversation.Online = _userService.GetUser(participants.Single(p => p != user))?.Online ?? false;

            return conversation;
        }



        private IList<Conversation> GetConversations()
        {
            return _cache.Get<IList<Conversation>>(ConversationsCacheKey);
        }
    }
}
