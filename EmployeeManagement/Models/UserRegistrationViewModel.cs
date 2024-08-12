using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Models
{
    public class UserRegistrationViewModel
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Phone")]
        public string Phone { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        //[Required]
        //[Display(Name = "Role")]
        //public string Role { get; set; }
    }
}
