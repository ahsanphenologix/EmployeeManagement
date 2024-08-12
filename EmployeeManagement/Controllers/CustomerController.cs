using EmployeeManagement.Models;
using EmployeeManagement.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    public class CustomerController : Controller
    {

        private CustomerServices _customerServices;
        private IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly string? _token;
        private readonly string? _role;

        public CustomerController(IConfiguration configuration, IHttpContextAccessor _contextAccessor) 
        {

            _customerServices = new CustomerServices(configuration, _contextAccessor);
            var session = _contextAccessor.HttpContext.Session;
            _token = session.GetString("JwtToken");
            _role = session.GetString("Role");
             
        } 

        // GET: CustomerController
        public IActionResult Index()
        {
            // Add the JWT token to the Authorization header
            

            if (_token == null)
                return RedirectToAction("Index", "Home");

            var customers = _customerServices.GetAllCustomers(_token).Result;

            return View(customers);
        }



        // GET: CustomerController/Create
        public ActionResult Create()
        {
            if (_token == null && _role != "Customer")
                return RedirectToAction("Index", "Home");

            return View();
        }

        // POST: CustomerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        public IActionResult Create(CustomerViewModel collection)
        {
            try
            {
                
                if (!ModelState.IsValid)
                    return View();

                if (_token == null && _role != "Customer")
                    return RedirectToAction("Index", "Home");

                _customerServices.AddCustomer(collection,_token).Wait();

                // Process the image here, e.g., save it to the server
                //if (collection.Image != null && collection.Image.Length > 0)
                //{
                //    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", collection.Image.FileName);

                //    using (var stream = new FileStream(filePath, FileMode.Create))
                //    {
                //        collection.Image.CopyTo(stream);
                //    }
                //}

                return ReturnToIndex();
            }
            catch(Exception ex)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: CustomerController/Edit/5
        public ActionResult Edit(int id)
        {
            if (_token == null && _role != "Admin")
                return RedirectToAction("Index", "Home");

            var customer = _customerServices.GetCustomerById(id,_token).Result;

            return View(new CustomerViewModel()
            {
                Id = customer.Id,
                Name = customer.Name,
                CustomerPostalCode = customer.CustomerPostalCode,
                City = customer.City,
                Email = customer.Email,
                Location = customer.Location,
                Phone = customer.Phone,
                Mobile = customer.Mobile,
                Image = null,
                Comment = customer.Comment
            });
        }

        // POST: CustomerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CustomerViewModel value)
        {
            try
            {
                if (_token == null && _role != "Admin")
                    return RedirectToAction("Index", "Home");

                _customerServices.UpdateCustomer(value,_token).Wait();

                return ReturnToIndex();
            }
            catch
            {
                return View("Index");
            }
        }

        // GET: CustomerController/Delete/5
        public IActionResult Delete(int id)
        {
            try
            {
                if (_token == null && _role != "Admin")
                    return RedirectToAction("Index", "Home");

                _customerServices.DeleteCustomer(id, _token);
                //return RedirectToAction(nameof(Index));

                return ReturnToIndex();
            }
            catch
            {
                return View("Index");
            }
        }

        // POST: CustomerController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        private IActionResult ReturnToIndex()
        {
            return RedirectToAction("Index");
        }
    }
}
