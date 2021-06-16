using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ChatTest.App.Models;

namespace ChatTest.App.Services
{
    public interface IConversationService
    {
        IEnumerable<ConversationModel> GetUserConversations(string userName);
        bool Exists(IEnumerable<string> participants, string name = null);
        ValueTask<Conversation> Create(string name, IEnumerable<string> participants, string userName, CancellationToken cancellationToken = default);
        Conversation Get(Guid conversationId, string userName);
        ValueTask Delete(Guid conversationId, CancellationToken cancellationToken = default);
        void MarkAsRead(Guid conversationId, string userName);
    }
}