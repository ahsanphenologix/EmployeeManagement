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

        public CustomerController(IConfiguration configuration) 
        { 
            _customerServices = new CustomerServices(configuration); 
        } 

        // GET: CustomerController
        public IActionResult Index()
        {
            var customers = _customerServices.GetAllCustomers().Result;

            return View(customers);
        }



        // GET: CustomerController/Create
        public ActionResult Create()
        {
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

                _customerServices.AddCustomer(collection).Wait();

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
            var customer = _customerServices.GetCustomerById(id).Result;

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
                _customerServices.UpdateCustomer(value).Wait();

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
                _customerServices.DeleteCustomer(id);
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
