using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.API.Models.RequestModels
{
    public class UserRegisterRequestModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Address { get; set; }
        //[Required]
        //public string? Role { get; set; }
    }
}
