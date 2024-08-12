using EmployeeManagement.Models;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmployeeManagement.Services
{
    public class AccountService
    {
        private readonly HttpClient _httpClient; //= new HttpClient();
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly string _apiUrl;

        public AccountService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = new HttpClient();
            _configuration = configuration;
            _apiUrl = _configuration["ApiSettings:BaseUrl"] + "Auth/";
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<CustomerResponseModel>> GetAllCustomers()
        {
            try
            {
                var response = await _httpClient.GetAsync(_apiUrl + "getall");
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


        public async Task<bool> UserRegister(UserRegistrationViewModel user)
        {
            try
            {
                bool isCreated = false;

                string url = _apiUrl + "register";


                // Serialize the object to JSON
                string jsonString = JsonConvert.SerializeObject(user);

                var jsonContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

                // Make the POST request
                HttpResponseMessage response = await _httpClient.PostAsync(url, jsonContent);

                response.EnsureSuccessStatusCode();

                // Check the response status
                if (response.IsSuccessStatusCode)
                {

                    isCreated = true;
                }
                else
                {
                    Console.WriteLine("Error: " + response.StatusCode);
                    
                }

                return isCreated;
                //var responseBody = await response.Content.ReadFromJsonAsync<List<CustomerModel>>();
                //return responseBody.ToList();
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                //return new CustomerModel();
                return false;
            }
        }
        
        public async Task<bool> UserLogin(LoginViewModel user)
        {
            try
            {
                bool isLogin = false;

                string url = _apiUrl + "login";


                // Serialize the object to JSON
                string jsonString = JsonConvert.SerializeObject(user);

                var jsonContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

                // Make the POST request
                HttpResponseMessage response = await _httpClient.PostAsync(url, jsonContent);

                response.EnsureSuccessStatusCode();

                // Check the response status
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var tokenResponse = JsonConvert.DeserializeObject<TokenResponseModel>(responseBody);

                    Console.WriteLine("Response: " + responseBody);

                    DecodeJwtAndAddToSession(tokenResponse.Token);


                    isLogin = true;
                }
                else
                {
                    Console.WriteLine("Error: " + response.StatusCode);
                    
                }

                return isLogin;
                //var responseBody = await response.Content.ReadFromJsonAsync<List<CustomerModel>>();
                //return responseBody.ToList();
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                //return new CustomerModel();
                return false;
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

        private async void DecodeJwtAndAddToSession(string token)
        {
            var session = _httpContextAccessor.HttpContext.Session;

            session.SetString("JwtToken", token);

            var handler = new JwtSecurityTokenHandler();

            var jwt = handler.ReadJwtToken(token);

            var claims = jwt.Claims
                .Where(claim => claim.Type == ClaimTypes.Role)
                .Select(claim => claim.Value).ToList();

            session.SetString("Role", claims.FirstOrDefault());
        }

    }
}
