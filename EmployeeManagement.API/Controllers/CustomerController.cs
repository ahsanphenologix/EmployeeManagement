using EmployeeManagement.API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using EmployeeManagement.API.Models.DatabaseModels;
using EmployeeManagement.API.Models;
using EmployeeManagement.API.Repository;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO.Pipelines;
using System.Runtime.InteropServices;
using static System.Net.Mime.MediaTypeNames;



// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EmployeeManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IRepository<CustomerDbModel> _customerRepository;
        private readonly IRepository<CustomerImageDbModel> _customerImageRepository;

        public CustomerController(IRepository<CustomerDbModel> repository, IRepository<CustomerImageDbModel> customerImageRepository)
        {
            _customerRepository = repository;
            _customerImageRepository = customerImageRepository;
        }

        // GET: api/<CustomerController>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var customers = await _customerRepository.GetAll();
                var images = await _customerImageRepository.GetAll();

                List<CustomerResponseModel> responseModel = new List<CustomerResponseModel>();

                foreach (var cust in customers) 
                {
                    string imagePath = null;

                    if (cust.ImageId == 0)
                        imagePath = string.Empty;
                    else
                        imagePath = "/Uploads/Images/" + images.Where(img => img.Id == cust.ImageId).First().Name;

                    responseModel.Add(new CustomerResponseModel()
                        {
                           Id = Convert.ToInt32(cust.Id),
                           Name = cust.Name,
                           Email = cust.Email,
                           Phone = cust.Phone,
                           City = cust.City,
                           Location = cust.Location,
                           CustomerPostalCode = cust.CustomerPostalCode,
                           Mobile = cust.Mobile,
                           Comment = cust.Comment,
                           ImagePath = imagePath,
                        });
                }

                //from image in images
                //where image.Id == (from customer in customers select customer.ImageId).FirstOrDefault()
                //select new
                //{
                //    Id = customer.Id,
                //    Name = customer.Name,
                //}


                return Ok(responseModel);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        // GET api/<CustomerController>/5
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var customer = await _customerRepository.GetById(id);
                var image = await _customerImageRepository.GetById(customer.ImageId);

                string imagePath = null;

                if (customer.ImageId == 0)
                    imagePath = string.Empty;
                else
                    imagePath = "/Uploads/Images/" + image.Name;

                var response = new CustomerResponseModel()
                {
                    Id = Convert.ToInt32(customer.Id),
                    Name = customer.Name,
                    Email = customer.Email,
                    Phone = customer.Phone,
                    City = customer.City,
                    Location = customer.Location,
                    CustomerPostalCode = customer.CustomerPostalCode,
                    Mobile = customer.Mobile,
                    Comment = customer.Comment,
                    ImagePath = imagePath,
                };



                return Ok(response);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        // POST api/<CustomerController>
        [HttpPost("AddNewCustomer")]
        public async Task<IActionResult> AddNewCustomerAsync([FromBody] CustomerRequestModel value)
        {
            try
            {
                int imageStoreId = await Convert64ToImageAndSave(value.Image); ;


                var customer = new CustomerDbModel
                {

                    Name = value.Name,
                    City = value.City,
                    Comment = value.Comment,
                    Email = value.Email,
                    CustomerPostalCode = value.CustomerPostalCode,
                    Location = value.Location,
                    Phone = value.Phone,
                    Mobile = value.Mobile,
                    ImageId = imageStoreId
                };

                

                var response = await _customerRepository.Insert(customer);
                if(response > 0)
                    return Ok(
                        new ResponseModel
                        {  
                            Response = response,
                            Message = "Customer inserted successfully"
                        });

                return StatusCode(500);
            }
            catch (Exception ex)
            {

                throw ex;

                return BadRequest(ex);
            }
        }

        // PUT api/<CustomerController>/5
        [HttpPost("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CustomerDbModel value)
        {
            try
            {
                var response = await _customerRepository.Update(id,value);

                return Ok(
                    new ResponseModel
                    {
                        Response = response,
                        Message = "Customer updated successfully"
                    });
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }

        // DELETE api/<CustomerController>/5
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _customerRepository.Delete(id);

                return Ok(
                    new ResponseModel
                    {
                        Response = response,
                        Message = "Customer deleted successfully"
                    });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        [NonAction]
        private async Task<int> Convert64ToImageAndSave(string base64Image)
        {
            string directoryPath = "~/Uploads/Images"; //Path.Combine(Directory.GetCurrentDirectory(), "Uploads/Images");
            Directory.CreateDirectory(directoryPath);  // Ensure the directory exists

            string imageName = "image" + Guid.NewGuid().ToString() + ".jpg";
            string uploadPath = Path.Combine(directoryPath, imageName);

            byte[] bytes = Convert.FromBase64String(base64Image);
            System.Drawing.Image image = null;

            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = new Bitmap(System.Drawing.Image.FromStream(ms));

                // Check if the image is valid
                if (image == null)
                {
                    throw new InvalidOperationException("Image object is not valid.");
                }
            }

            try
            {
                // Save the image to the specified path
                image.Save(uploadPath, ImageFormat.Jpeg);
            }
            catch (ExternalException ex)
            {
                Console.WriteLine($"Image saving failed: {ex.Message}");
                throw;
            }
            catch (IOException ex)
            {
                Console.WriteLine($"File IO error: {ex.Message}");
                throw;
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Access error: {ex.Message}");
                throw;
            }

            var imagestoreid = await _customerImageRepository.Insert(new CustomerImageDbModel()
            {
                Name = imageName,
                Path = uploadPath,
            });

            return imagestoreid;
        }
    }
}
