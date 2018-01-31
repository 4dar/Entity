using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using bankAccounts.Models;
using System.Linq;
namespace bankAccounts.Controllers
{
    public class AccountController : Controller
    {
        private UserContext _context;
        public AccountController(UserContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("home")]
        public IActionResult home()
        {
            int? id = HttpContext.Session.GetInt32("userId"); // check for User's ID in session
            if (id == null) // if user is not in session, return to Login Page
            {
                return RedirectToAction("loginPage", "User");
            }
            List<Transaction> Transactions = _context.Transactions.Where(t => t.UserId == id).ToList(); // else, find all transactions belonging to logged in User
            TempData["userName"] = HttpContext.Session.GetString("userName"); // get User's name from session (was set in login/reg)
            User loggedInUser = _context.Users.Where(u => u.UserId == id).SingleOrDefault(); // hold the object of the logged in user
            TempData["balance"] = loggedInUser.Balance; // get logged in User's "Balance"
            ViewBag.transactions = Transactions; // front-end access of object of all User's transactions
            return View();
        }

        [HttpPost]
        [Route("createTransaction")]
        public IActionResult CreateTransaction(double amount)
        {
            if (amount != 0) // if an amount was added or subtracted: 
            {
                int? id = HttpContext.Session.GetInt32("userId"); // check for User's ID in session
                if (id == null) // if user is not in session, return to Login Page
                {
                    return RedirectToAction("loginPage", "User");
                }
                Transaction newTransaction = new Transaction
                {
                    UserId = (int)id, // set the UserID column in transactions to equal User's main ID
                    Amount = amount,
                    Date = DateTime.Now
                }; // enter data into Transactions table in DB
                User user = _context.Users.Where(u => u.UserId == newTransaction.UserId).SingleOrDefault(); // get user object who's transaction will be made
                user.Balance += (double)newTransaction.Amount; // set User's balance to add or subtract the new amount in current transaction
                _context.Add(newTransaction); // commit
                _context.SaveChanges();
                return RedirectToAction("home");
            }
            else
            {
                return View("home"); // if amount was 0, stay at home
            }
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "User");
        }
    }
}