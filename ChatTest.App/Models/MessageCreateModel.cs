using System.ComponentModel.DataAnnotations;

namespace ChatTest.App.Models
{
    public class MessageCreateModel
    {
        [Required]
        public string Text { get; set; }
    }
}
