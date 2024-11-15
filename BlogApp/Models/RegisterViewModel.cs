using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string UserName { get; set; } = null!;

        [Required]
        public string? Name { get; set; }
    
       
        [Required]
        [EmailAddress]
        [Display(Name ="Email    :")]
        public string? Email { get; set; }


        [Display(Name = "Password :")]
        [Required]
        [StringLength(15, ErrorMessage = "Parolanız en fazla 15 en az 5 karakterli olmalıdır", MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string? Password { get; set; }[Display(Name ="Password :")]


        [Required]
        [StringLength(15,ErrorMessage ="Parolanız en fazla 15 en az 5 karakterli olmalıdır",MinimumLength =5)]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage ="Parolanız Eşleşmiyor!")]
        public string? ConfirmPassword { get; set; } 
    }
}
