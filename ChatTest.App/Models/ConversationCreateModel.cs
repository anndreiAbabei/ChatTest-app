using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChatTest.App.Models
{
    public class ConversationCreateModel
    {
        public string Name { get; set; }

        [Required]
        public IEnumerable<string> Participants { get; set; }
    }
}
