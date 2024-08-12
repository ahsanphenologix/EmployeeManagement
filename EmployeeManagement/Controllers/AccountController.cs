using Microsoft.AspNetCore.Mvc;
using EmployeeManagement.Models;
using EmployeeManagement.Services;

namespace EmployeeManagement.Controllers
{
    public class AccountController : Controller
    {
        
        private AccountService _accountService;
        //private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly IConfiguration _configuration;

        public AccountController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _accountService = new AccountService(configuration, httpContextAccessor);
            //_httpContextAccessor = httpContextAccessor;
            //_configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserRegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var isRegister = _accountService.UserRegister(model).Result;
                // Redirect or return a view after successful registration
                if(isRegister)
                    return RedirectToAction("Index", "Home");
            }

            return View(model);
        }
        
        
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var isRegister = _accountService.UserLogin(model).Result;
                // Redirect or return a view after successful registration
                if(isRegister)
                    return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            // Clear the session
            HttpContext.Session.Remove("JwtToken");
            //HttpContext.Session.Remove("UserRole");

            // Optionally redirect to the home page or login page
            return RedirectToAction("Index", "Home");
        }
    }
}
