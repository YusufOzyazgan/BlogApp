using BlogApp.Entity;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class EditPostViewModel
    {
        public int PostId { get; set; }
        public bool IsActive { get; set; }
        [Required]
        public string? Title { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public string? Content { get; set; }
        
        public IFormFile? Image { get; set; }
        public string? ImageUrl { get; set; }

        [Required]
        public string? Url { get; set; }
        public List<Tag>? Tags { get; set; }
    }
}
