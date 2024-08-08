using EmployeeManagement.Models;
using EmployeeManagement.Models.DatabaseModels;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Text;

namespace EmployeeManagement.Services
{
    public class CustomerServices
    {
        private readonly HttpClient _httpClient; //= new HttpClient();
        private readonly IConfiguration _configuration;
        private readonly string _apiUrl = string.Empty;
        public CustomerServices(IConfiguration configuration) 
        {
            _httpClient = new HttpClient();
            _configuration = configuration;
            _apiUrl = _configuration["ApiSettings:BaseUrl"];
        }

        public async Task<List<CustomerResponseModel>> GetAllCustomers()
        {
            try
            {
                var response = await _httpClient.GetAsync(_apiUrl+"getall");
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadFromJsonAsync<List<CustomerResponseModel>>();
                return responseBody.ToList();
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                return new List<CustomerResponseModel>();
            }
        }
        
        
        public async Task AddCustomer(CustomerViewModel customer)
        {
            try
            {
                

                string url = _apiUrl + "AddNewCustomer";
                
                
                string imageString = string.Empty;


                using (var memoryStream = new MemoryStream())
                {
                    await customer.Image.CopyToAsync(memoryStream);
                    byte[] imageData = memoryStream.ToArray();
                    var imageContent = Convert.ToBase64String(imageData);  
                    imageString = imageContent;
                }

          
                var cust = new
                {

                    Name = customer.Name,

                    CustomerPostalCode = customer.CustomerPostalCode,

                    City = customer.City,

                    Email = customer.Email,

                    Location = customer.Location,

                    Phone = customer.Phone,

                    Mobile = customer.Mobile,

                    Image = imageString,

                    Comment = customer.Comment
                };

                // Serialize the object to JSON
                string jsonString  = JsonConvert.SerializeObject(cust);

                var jsonContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

                // Make the POST request
                HttpResponseMessage response = await _httpClient.PostAsync(url, jsonContent);

                response.EnsureSuccessStatusCode();

                // Check the response status
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    Console.WriteLine("Response: " + responseBody);
                }
                else
                {
                    Console.WriteLine("Error: " + response.StatusCode);
                }


                //var responseBody = await response.Content.ReadFromJsonAsync<List<CustomerModel>>();
                //return responseBody.ToList();
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                //return new CustomerModel();
            }
        }
    
    
        public async Task<CustomerResponseModel> GetCustomerById(int id)
        {
            try
            {

                var uriBuilder = new UriBuilder(_apiUrl + $"GetById/{id}");
                //var query = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);
                //query["Id"] = id.ToString();
             
                //uriBuilder.Query = query.ToString();

                //string url = uriBuilder.ToString();

                //HttpResponseMessage response = await _httpClient.GetAsync(url);
                HttpResponseMessage response = await _httpClient.GetAsync(uriBuilder.ToString());
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadFromJsonAsync<CustomerResponseModel>();

          
                
                return responseBody;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                return new CustomerResponseModel();
            }
        }

        public async Task UpdateCustomer(CustomerViewModel customer) 
        {
            try
            {


                string url = _apiUrl + $"Update/{customer.Id}";


                string imageString = string.Empty;


                //using (var memoryStream = new MemoryStream())
                //{
                //    await customer.Image.CopyToAsync(memoryStream);
                //    byte[] imageData = memoryStream.ToArray();
                //    var imageContent = Convert.ToBase64String(imageData);
                //    imageString = imageContent;
                //}


                var cust = new
                {

                    Name = customer.Name,

                    CustomerPostalCode = customer.CustomerPostalCode,

                    City = customer.City,

                    Email = customer.Email,

                    Location = customer.Location,

                    Phone = customer.Phone,

                    Mobile = customer.Mobile,

                    Image = imageString,

                    Comment = customer.Comment
                };

                // Serialize the object to JSON
                string jsonString = JsonConvert.SerializeObject(cust);

                var jsonContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

                // Make the POST request
                HttpResponseMessage response = await _httpClient.PostAsync(url, jsonContent);

                response.EnsureSuccessStatusCode();

                // Check the response status
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    Console.WriteLine("Response: " + responseBody);
                }
                else
                {
                    Console.WriteLine("Error: " + response.StatusCode);
                }


                //var responseBody = await response.Content.ReadFromJsonAsync<List<CustomerModel>>();
                //return responseBody.ToList();
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                //return new CustomerModel();
            }
        }

        public void DeleteCustomer(int id) 
        {
            try
            {

                var uriBuilder = new UriBuilder(_apiUrl + $"Delete/{id}");
                

                //HttpResponseMessage response = await _httpClient.GetAsync(url);
                HttpResponseMessage response = _httpClient.DeleteAsync(uriBuilder.ToString()).Result;
                //response.EnsureSuccessStatusCode();
                var responseBody = response.Content.ReadFromJsonAsync<CustomerResponseModel>().Result;



                //return responseBody.i
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
               
            }

        }

    }
}
