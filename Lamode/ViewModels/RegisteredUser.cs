using System;
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
        public Nullable<System.DateTime> DateOfBirth { get; set; }

        public Nullable<decimal> Height { get; set; }
        public Nullable<decimal> Weight { get; set; }
        public string ColorEyes { get; set; }

        [Display(Name = "Tell Us More")]
        public string TellUsMore { get; set; }
        public Nullable<bool> Experience { get; set; }

        [Display(Name = "Nude Photo")]
        public Nullable<bool> NudePhoto { get; set; }
        public string Nationality { get; set; }
        public string country { get; set; }
        public string City { get; set; }
        public string state { get; set; }
        public string ZipCode { get; set; }
        public Nullable<decimal> Bust { get; set; }
        public Nullable<decimal> Hips { get; set; }
        public Nullable<decimal> Dress { get; set; }
        public Nullable<decimal> Waist { get; set; }
        public Nullable<decimal> Cup { get; set; }
        public Nullable<decimal> Shoe { get; set; }

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }
        public string Gender { get; set; }
        public string Website { get; set; }

    }
}
