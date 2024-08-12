
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.API.Models.DatabaseModels
{
    public class ApplicationUser : IdentityUser
    {
        //UserName
        //Email
        //PhoneNumber
        //Password

        public string Name { get; set; } 
    
        public string Address { get; set; }
        
        //public string? Role { get; set; }
    }
}
