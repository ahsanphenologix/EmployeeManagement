using EmployeeManagement.API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using EmployeeManagement.API.Models.DatabaseModels;
using EmployeeManagement.API.Models;
using EmployeeManagement.API.Repository;
using System.IO;
using System.Drawing.Imaging;
using System.IO.Pipelines;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Authorization;
//using Microsoft.Extensions.PlatformAbstractions;



// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EmployeeManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IRepository<CustomerDbModel> _customerRepository;
        private readonly IRepository<CustomerImageDbModel> _customerImageRepository;

        //private readonly IApplicationEnvironment _applicationEnvironment;

        public CustomerController(IRepository<CustomerDbModel> repository, IRepository<CustomerImageDbModel> customerImageRepository)
        {
            _customerRepository = repository;
            _customerImageRepository = customerImageRepository;
        }

        [Authorize]
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

                    if (cust.ImageId < 5)
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


                return Ok(responseModel);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [Authorize( Roles = "Admin")]
        // GET api/<CustomerController>/5
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var customer = await _customerRepository.GetById(id);

                if (customer == null)
                    return NoContent();

                var image = await _customerImageRepository.GetById(customer.ImageId);

                string imagePath = null;

                if (customer.ImageId == 0 || image == null)
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

        [Authorize(Roles = "Customer")]
        // POST api/<CustomerController>
        [HttpPost("AddNewCustomer")]
        public async Task<IActionResult> AddNewCustomerAsync([FromBody] CustomerRequestModel value)
        {
            try
            {
                int imageStoreId = await Convert64ToImageAndSave(value.Image,value.ImageName); 


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

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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
        private async Task<int> Convert64ToImageAndSave(string base64Image,string imageName)
        {

            
            string directoryPath = "wwwroot/Uploads/Images"; //Path.Combine(Directory.GetCurrentDirectory(), "Uploads/Images");
            Directory.CreateDirectory(directoryPath);  // Ensure the directory exists

            string extention = imageName.Split('.')[1];
            string imageNewName = "image" + Guid.NewGuid().ToString() + "." + extention;
            string uploadPath = Path.Combine(directoryPath, imageNewName);

            byte[] bytes = Convert.FromBase64String(base64Image);

           
            using (MemoryStream ms = new MemoryStream(bytes, 0 , bytes.Length))
            {
                using (var image = System.Drawing.Image.FromStream(ms))
                {

                   

                    // Check if the image is valid
                    if (image == null)
                    {
                        throw new InvalidOperationException("Image object is not valid.");
                    }

                    try
                    {
                        ImageFormat imageFormat = null;

                        // Save the image to the specified path
                        if (extention == "jpg" || extention == "jpeg")
                            imageFormat = ImageFormat.Jpeg;
                        else if (extention == "png")
                            imageFormat = ImageFormat.Png;

                        if (imageFormat == null)
                            return 5;

                        image.Save(uploadPath, imageFormat);

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

                }
            }

            

            var imagestoreid = await _customerImageRepository.Insert(new CustomerImageDbModel()
            {
                Name = imageNewName,
                Path = uploadPath,
            });

            return imagestoreid;
        }
    }
}
