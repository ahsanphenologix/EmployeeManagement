namespace EmployeeManagement.API.Models
{
    public class CustomerResponseModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public long CustomerPostalCode { get; set; }

        //public string Address { get; set; }
        public string City { get; set; }

        public string Email { get; set; }

        public string Location { get; set; }

        public string Phone { get; set; }
        public string Mobile { get; set; }

        public string ImagePath { get; set; }

        public string Comment { get; set; }
    }
}
