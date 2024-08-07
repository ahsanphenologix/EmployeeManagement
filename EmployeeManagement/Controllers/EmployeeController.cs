using EmployeeManagement.AppDbContext;
using EmployeeManagement.Models.DatabaseModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeMDbContext _employeeDbContext;

        public EmployeeController(EmployeeMDbContext employeeDbContext)
        {
            _employeeDbContext = employeeDbContext;
        }

        public IActionResult Index()
        {
            var employees = _employeeDbContext.Employees.ToList();

            return View(employees);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(EmployeeModel employee)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();
                

                _employeeDbContext.Add(employee);
                _employeeDbContext.SaveChanges();
                return ReturnToIndex();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }


        public IActionResult Edit(int id)
        {
            try
            {
                var employee = _employeeDbContext.Employees.SingleOrDefault(x => x.Id == id);

                return View(employee);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [HttpPost]
        public IActionResult Edit(EmployeeModel employee)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();


                _employeeDbContext.Update(employee);
                _employeeDbContext.SaveChanges();
                return ReturnToIndex();

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public IActionResult Delete(int id)
        {
            try
            {
                
                var emp = _employeeDbContext.Employees.Single(e => e.Id == id);

              
                _employeeDbContext.Employees.Remove(emp);
                _employeeDbContext.SaveChanges();

                return ReturnToIndex();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }



        private IActionResult ReturnToIndex()
        {
            return RedirectToAction("Index");
        }

    }
}
