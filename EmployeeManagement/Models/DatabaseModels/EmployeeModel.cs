using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Models.DatabaseModels
{
    public class EmployeeModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string CompanyName { get; set; }
        public string Designation { get; set; }
    }
}
