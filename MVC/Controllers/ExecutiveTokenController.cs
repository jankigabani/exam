using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Repositories.Models;
using Repositories.Repository;

namespace MVC.Controllers
{
    // [Route("[controller]")]
    public class ExecutiveTokenController : Controller
    {
        private readonly ITempRepository _executiveRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ExecutiveTokenController(ITempRepository executiveRepo, IHttpContextAccessor httpContextAccessor)
        {
            _executiveRepo = executiveRepo;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index(tblExecutiveLogin executive)
        {
            string login = _httpContextAccessor.HttpContext.Session.GetString("usertype");
            ViewBag.session = login;

            var tokens = _executiveRepo.GetAll(login);
            return View(tokens);
        }


        [HttpGet]
        public IActionResult ExecutiveLogin()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(tblExecutiveLogin login)
        {
            var loggedInUser = _executiveRepo.Login(login);
            if (loggedInUser != null)
            {
                // Store user data in session
                _httpContextAccessor.HttpContext.Session.SetString("userid", loggedInUser.c_userid);
                _httpContextAccessor.HttpContext.Session.SetString("upassword", loggedInUser.c_password);

                _httpContextAccessor.HttpContext.Session.SetString("usertype", loggedInUser.token_type);
                return RedirectToAction("Index", "ExecutiveToken"); // Redirect to the index page of HomeController
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid username or password."; // Provide error message
                return RedirectToAction("ExecutiveLogin");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}