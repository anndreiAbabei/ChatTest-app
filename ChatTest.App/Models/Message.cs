using System;

namespace ChatTest.App.Models
{
    public class Message
    {
        public Guid  Id { get; set; }

        public Guid ConversationId { get; set; }

        public string Text { get; set; }

        public string Sender { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
