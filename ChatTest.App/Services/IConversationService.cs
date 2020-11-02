using System;
using System.Collections.Generic;
using ChatTest.App.Models;

namespace ChatTest.App.Services
{
    public interface IConversationService : ISeeder
    {
        IEnumerable<ConversationModel> GetUserConversations(string userName);
        bool Exists(IEnumerable<string> participants, string name = null);
        Conversation Create(string name, IEnumerable<string> participants, string userName);
        Conversation Get(Guid conversationId, string userName);
        void Delete(Guid conversationId);
        void MarkAsRead(Guid conversationId, string userName);
    }
}