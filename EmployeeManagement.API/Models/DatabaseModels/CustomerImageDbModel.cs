using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.API.Models.DatabaseModels
{
    public class CustomerImageDbModel
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }
    }
}
