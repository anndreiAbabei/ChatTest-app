using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ChatTest.App.Models;

namespace ChatTest.App.Services
{
    public interface IMessangesService
    {
        IEnumerable<MessageModel> GetMessages(Guid conversationId, string userName);
        ValueTask Create(string userName, Guid conversationId, string text, CancellationToken cancellationToken = default);
    }
}