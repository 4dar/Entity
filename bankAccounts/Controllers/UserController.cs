using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;
using bankAccounts.Models;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace bankAccounts.Controllers
{
    public class UserController : Controller
    {
        private UserContext _context;
 
        public UserController(UserContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View("Index");
        }


        [HttpGet]
        [Route("login")]
        public IActionResult loginPage()
        {
            return View("login");
        }

        [HttpPost]
        [Route("signin")]
        public IActionResult signin(LoginUser model)
        {
            if(ModelState.IsValid)
            {
                User exists = _context.Users.SingleOrDefault(u => u.Email == model.login_email); // check if User exists in DB
                if(exists == null)
                {
                    ModelState.AddModelError("Email", "Email not found");
                    return View("login");
                }
                else
                {
                    var hasher = new PasswordHasher<User>();
                    if(hasher.VerifyHashedPassword(exists, exists.Password, model.login_password) == 0)
                    {
                        ModelState.AddModelError("Password", "Incorrect password");
                        return View("login");
                    }
                    User user = _context.Users.Where(u => u.Email == model.login_email).SingleOrDefault(); // hold user object by email
                    HttpContext.Session.SetInt32("userId", user.UserId); // set user's ID in session
                    HttpContext.Session.SetString("userName", user.Name); // set user's name in session
                    return RedirectToAction("home", "Account"); // go to home page
                }
            }
            return View("login");
        }


        [HttpPost]
        [Route("registerUser")]
        public IActionResult registerUser(RegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                User exists = _context.Users.SingleOrDefault(user => user.Email == model.Email); // check if user exists
                if(exists != null)
                {
                    ModelState.AddModelError("Email", "An account with this email already exists!");
                    return View("Index");
                }
                else
                {
                    PasswordHasher<RegisterViewModel> Hasher = new PasswordHasher<RegisterViewModel>();
                    string hashed = Hasher.HashPassword(model, model.Password);

                    User newUser = new User
                    {
                        Name = model.Name,
                        Email = model.Email,  
                        Password = hashed,
                        Balance = 0.00
                    }; // save user into DB, and have Balance start at zero
                    _context.Add(newUser);
                    _context.SaveChanges();

                    User user = _context.Users.Where(u => u.Email == model.Email).SingleOrDefault(); // get and hold user object by email
                    HttpContext.Session.SetInt32("userId", user.UserId); // set user's ID in session
                    HttpContext.Session.SetString("userName", user.Name); // set user's name in session
                    return RedirectToAction("home", "Account"); // go to home page!
                }
                
                }
                else
                {
                    return View("Index");
                }
            }

    }

}
    
