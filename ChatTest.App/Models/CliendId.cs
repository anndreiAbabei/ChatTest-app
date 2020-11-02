using System.ComponentModel.DataAnnotations;

namespace ChatTest.App.Models
{
    public class CliendId
    {
        [Required]
        public string Id { get; set; }
        
        [Required]
        public string ConnectionId { get; set; }
    }
}
