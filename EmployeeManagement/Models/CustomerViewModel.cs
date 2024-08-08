using static System.Formats.Asn1.AsnWriter;

namespace EmployeeManagement.Models
{
    public class CustomerViewModel
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public long CustomerPostalCode { get; set; }

        //public string Address { get; set; }
        public string City { get; set; }

        public string Email { get; set; }

        public string Location { get; set; }

        public string Phone { get; set; }
        public string Mobile { get; set; }

        public IFormFile? Image { get; set; }

        public string Comment { get; set; }

    }
}
