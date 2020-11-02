using System;
using System.Collections.Generic;
using System.Linq;
using ChatTest.App.Models;
using Microsoft.Extensions.Caching.Memory;

namespace ChatTest.App.Services
{
    public class ConversationService : IConversationService
    {
        private readonly IMemoryCache _cache;
        private readonly IUserService _userService;
        private const string ConversationsCacheKey = "conversations";
        private static readonly object SyncRoot = new object();



        public ConversationService(IMemoryCache cache, IUserService userService)
        {
            _cache = cache;
            _userService = userService;
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

            return conversations.Any(c => c.Participants.SequenceEqual(participants) &&
                                          (string.IsNullOrEmpty(userName) || c.Name == userName));
        }



        public Conversation Get(Guid conversationId, string userName)
        {
            return FillConversation(GetConversations().FirstOrDefault(c => c.Id == conversationId), 
                                    userName);
        }



        public Conversation Create(string name, IEnumerable<string> participants, string userName)
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

            return newConv;
        }



        public void MarkAsRead(Guid conversationId, string userName)
        {
            Get(conversationId, userName)?.Reads.TryAdd(userName, DateTime.UtcNow);
        }



        public void Delete(Guid conversationId)
        {
            IList<Conversation> conversations = GetConversations();
            Conversation conv = conversations.FirstOrDefault(c => c.Id == conversationId);

            if (conv != null)
                conversations.Remove(conv);
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



        void ISeeder.Seed()
        {
            lock (SyncRoot)
            {
                var list = new List<Conversation>();
                if (Constants.Mock.UseMock)
                {

                    list.Add(new Conversation
                             {
                                 Id = Constants.Mock.ConversationIds[0],
                                 CreatedAt = DateTime.UtcNow,
                                 CreatedBy = Constants.Mock.UserNames[0],
                                 Participants = new []
                                                {
                                                    Constants.Mock.UserNames[0],
                                                    Constants.Mock.UserNames[1]
                                                }
                             });

                    list.Add(new Conversation
                             {
                                 Id = Constants.Mock.ConversationIds[1],
                                 CreatedAt = DateTime.UtcNow,
                                 CreatedBy = Constants.Mock.UserNames[0],
                                 Participants = new []
                                                {
                                                    Constants.Mock.UserNames[0],
                                                    Constants.Mock.UserNames[2]
                                                }
                             });

                    list.Add(new Conversation
                             {
                                 Id = Constants.Mock.ConversationIds[2],
                                 CreatedAt = DateTime.UtcNow,
                                 CreatedBy = Constants.Mock.UserNames[0],
                                 Participants = new []
                                                {
                                                    Constants.Mock.UserNames[0],
                                                    Constants.Mock.UserNames[1],
                                                    Constants.Mock.UserNames[2]
                                                }
                             });
                }
                _cache.Set<IList<Conversation>>(ConversationsCacheKey, list);
            }
        }
    }
}
