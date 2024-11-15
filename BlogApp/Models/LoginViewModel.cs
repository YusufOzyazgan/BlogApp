using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name ="Email    :")]
        public string? Email { get; set; }


        [Display(Name ="Password :")]
        [Required]
        [StringLength(15,ErrorMessage ="Parolanız en fazla 15 en az 5 karakterli olmalıdır",MinimumLength =5)]
        [DataType(DataType.Password)]
        public string? Password { get; set; }    
    }
}
