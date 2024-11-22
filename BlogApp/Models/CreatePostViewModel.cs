using BlogApp.Entity;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class CreatePostViewModel
    {
        
        public int UserId { get; set; }

        [Required]
        public string? Title { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public string? Content { get; set; }
        
        public string? Image { get; set; }
        [Required]
        public string? Url { get; set; }
       
    }
}
