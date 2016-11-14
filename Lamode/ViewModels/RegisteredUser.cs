using System.ComponentModel.DataAnnotations;

namespace Lamode.ViewModels
{
    public class RegisteredUser
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$",
        ErrorMessage = "This is not a valid email address.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password Confirm")]
        [Compare("Password", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        [Required]
        [Display(Name = "Date Of Birth")]
        public string DateOfBirth { get; set; }

        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public string ColorEyes { get; set; }

        [Display(Name = "Tell Us More")]
        public string TellUsMore { get; set; }
        public bool Experience { get; set; }

        [Display(Name = "Nude Photo")]
        public bool NudePhoto { get; set; }
        public string Nationality { get; set; }
        public string ZipCode { get; set; }
        public decimal Bust { get; set; }
        public decimal Hips { get; set; }
        public decimal Dress { get; set; }
        public decimal Waist { get; set; }
        public decimal Cup { get; set; }
        public decimal Shoe { get; set; }

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }
        public string Website { get; set; }

    }
}
