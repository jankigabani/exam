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
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserRepository _userRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public UserController(ILogger<UserController> logger, IUserRepository userRepo, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _userRepo = userRepo;
            _httpContextAccessor = httpContextAccessor;

        }
        [HttpGet]
        public IActionResult Index()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            if (string.IsNullOrEmpty(session.GetString("username")))
            {
                return RedirectToAction("Login");

            }
            var trips = _userRepo.GetAll(session.GetString("userid"));
            return View(trips);

        }

        public IActionResult Dashboard()
        {
            var session = HttpContext.Session;
            if (string.IsNullOrEmpty(session.GetString("username")))
            {
                return RedirectToAction("Login", "User");
            }
            var userId = session.GetInt32("userid");

            List<tblAdmin> allitems = _userRepo.GetAll(userId.ToString());
            return View(allitems);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(tblUser user)
        {
            tblUser verifiedUser = _userRepo.LoginDatabase(user);
            if (verifiedUser == null)
            {
                ViewBag.error = "Invalid Credential!";
                return View();
            }
            else
            {
                var session = _httpContextAccessor.HttpContext.Session;
                session.SetString("userid", verifiedUser.c_userid.ToString());
                session.SetString("email", verifiedUser.c_email);
                session.SetString("username", verifiedUser.c_username);
                Console.WriteLine(session.GetString("email"));

                // Check if the logged-in user is an admin
                if (verifiedUser.c_email == "admin@gmail.com" && verifiedUser.c_password == "12345")
                {
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    return RedirectToAction("Dashboard", "User");
                }
            }
        }

        public IActionResult Logout()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            session.Clear();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult InsertTrip()
        {
            return View();
        }

        [HttpPost]
        public IActionResult InsertTrip(tblAdmin admin)
        {
            _userRepo.InsertTrip(admin);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult GetTripDetails(int id)
        {
            var student = _userRepo.FetchTripDetails(id);
            return View(student);
        }

        public ActionResult DeleteTrip(int id)
        {
            var student = _userRepo.FetchTripDetails(id);
            return View(student);
        }
        [HttpPost]
        public IActionResult DeleteTripConfirmed(int id)
        {
            _userRepo.DeleteTripDetails(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult UpdateTrip(int Id) // Correct the parameter name to match the one in your route
        {
            var tripDetails = _userRepo.FetchTripDetails(Id); // Fetch trip details by Id
            return View(tripDetails);
        }

        [HttpPost]
        public IActionResult UpdateTrip(tblAdmin admin)
        {
            try
            {
                _userRepo.UpdateExistingTrip(admin); // Update trip details
                return RedirectToAction("Index"); // Redirect to index page upon successful update
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating trip: " + ex.Message); // Log error message
                return View(admin); // Return to the edit page with the submitted data
            }
        }

        [HttpGet]
        public IActionResult AddCustomerTrip(int id)
        {
            int userId = HttpContext.Session.GetInt32("userid") ?? 0;
            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("Login", "User");
            }

            ViewBag.UserId = userId;

            var TicketDetails = _userRepo.FetchTripDetails(id);

            var TicketBook = new tblCustomer
            {
                c_trip = TicketDetails.c_trip,
                c_price = TicketDetails.c_price,
                c_currentstock = TicketDetails.c_currentstock,
                c_userid = userId,
                c_ticketid = TicketDetails.c_ticketid,
            };

            return View(TicketBook);
        }

        [HttpPost]
        public IActionResult AddCustomerTrip(tblCustomer customer)
        {
            try
            {
                var tripDetails = _userRepo.FetchTripDetails(customer.c_ticketid);

                if (tripDetails.c_currentstock >= customer.c_ticketqnt)
                {
                    tripDetails.c_currentstock -= customer.c_ticketqnt;
                    _userRepo.UpdateExistingTrip(tripDetails);

                    _userRepo.InsertCustomerTrip(customer);
                    return RedirectToAction("Dashboard");
                }
                else
                {
                    TempData["ErrorMessage"] = "Not enough tickets available for booking!";
                    return RedirectToAction("Dashboard");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding customer trip: " + ex.Message);
                return RedirectToAction("Dashboard");
            }
        }


        [HttpGet]
        public IActionResult GetCustomerTripDetails(int id)
        {
            var customer = _userRepo.GetAllBookings(id);
            return View(customer);
        }

        [HttpPost]
        public IActionResult CancelTrip(int id)
        {
            try
            {
                _userRepo.UpdateBookingStatus(id, "Cancelled");
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error cancelling trip: " + ex.Message);
                return Json(new { success = false });
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}