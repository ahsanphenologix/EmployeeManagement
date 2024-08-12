﻿using EmployeeManagement.Models;
using EmployeeManagement.Models.DatabaseModels;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using NuGet.Common;

namespace EmployeeManagement.Services
{
    public class CustomerServices
    {
        private readonly HttpClient _httpClient; //= new HttpClient();
        private readonly IConfiguration _configuration;
        private readonly string _apiUrl = string.Empty;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomerServices(IConfiguration configuration, IHttpContextAccessor httpContextAccessor) 
        {
            _httpClient = new HttpClient();
            _configuration = configuration;
            //_httpContextAccessor = httpContextAccessor;
            _apiUrl =  _configuration["ApiSettings:BaseUrl"] + "customer/";
        }

        public async Task<List<CustomerResponseModel>> GetAllCustomers(string token)
        {
            try
            {

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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
        
        
        public async Task AddCustomer(CustomerViewModel customer, string token)
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

                    ImageName = customer.Image.FileName,

                    Comment = customer.Comment
                };

                // Serialize the object to JSON
                string jsonString  = JsonConvert.SerializeObject(cust);

                var jsonContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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
    
    
        public async Task<CustomerResponseModel> GetCustomerById(int id,string token)
        {
            try
            {

                var uriBuilder = new UriBuilder(_apiUrl + $"GetById/{id}");
                //var query = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);
                //query["Id"] = id.ToString();

                //uriBuilder.Query = query.ToString();

                //string url = uriBuilder.ToString();

                //HttpResponseMessage response = await _httpClient.GetAsync(url);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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

        public async Task UpdateCustomer(CustomerViewModel customer, string token) 
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
                    Id = customer.Id,
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

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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

        public void DeleteCustomer(int id, string token) 
        {
            try
            {

                var uriBuilder = new UriBuilder(_apiUrl + $"Delete/{id}");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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
