using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BankAccounts.Models;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System.Data.Entity;


namespace BankAccounts.Controllers
{
  public class HomeController : Controller
  {
    private BankAccountsContext dbContext;
    public HomeController(BankAccountsContext context)
    {
      dbContext = context;
    }


    [HttpGet]
    [Route("")]
    public IActionResult Index()
    {
      return View();
    }

    [HttpGet("Hacker")]
    public IActionResult Hacker()
    {
      return View();
    }


    [HttpGet]
    [Route("/success")]
    public IActionResult Success()
    {
      if (HttpContext.Session.GetInt32("UserId") == null)
      {
        return RedirectToAction("Hacker");
      }
      return View();
    }


    [HttpPost("RegisterUser")]
    public IActionResult RegisterUser(User user)
    {
      if (ModelState.IsValid)
      {
        if (dbContext.User.Any(u => u.Email == user.Email))
        {
          ModelState.AddModelError("Email", "Email already in use!");
          return View("Index");
        }
        else
        {
          PasswordHasher<User> Hasher = new PasswordHasher<User>();
          user.Password = Hasher.HashPassword(user, user.Password);

          dbContext.Add(user);
          dbContext.SaveChanges();
          var AddedUser = dbContext.User.FirstOrDefault(u => u.Email == user.Email);
          HttpContext.Session.SetInt32("UserId", AddedUser.UserId);
          int? LoggedInUserId = HttpContext.Session.GetInt32("UserId");

          return RedirectToAction("ViewProfile", new {id = LoggedInUserId});
        }
      }
      else
      {
        return View("Index");
      }
    }

    [HttpGet("ViewLoginUser")]
    public IActionResult ViewLoginUser()
    {
      return View();
    }

    [HttpPost("LoginUser")]
    public IActionResult LoginUser(LoginUser UserSubmission)
    {
      if (ModelState.IsValid)
      {
        var UserToCheck = dbContext.User.FirstOrDefault(u => u.Email == UserSubmission.Email);
        if (UserToCheck == null)
        {
          ModelState.AddModelError("Email", "Invalid Email or Password");
          return View("ViewLoginUser");
        }
        var hasher = new PasswordHasher<LoginUser>();

        var result = hasher.VerifyHashedPassword(UserSubmission, UserToCheck.Password, UserSubmission.Password);

        if (result == 0)
        {
          ModelState.AddModelError("Password", "Invalid Email or Password");
          return View("ViewLoginUser");
        }
        HttpContext.Session.SetInt32("UserId", UserToCheck.UserId);
        int? LoggedInUserId = HttpContext.Session.GetInt32("UserId");
        return RedirectToAction("ViewProfile", new {id = LoggedInUserId});
      }
      return View("ViewLoginUser");
    }

    [HttpGet("account/{id}")]
    public IActionResult ViewProfile(int id)
    {
        HttpContext.Session.SetInt32("UserId", id);
        int? LoggedInUserId = HttpContext.Session.GetInt32("UserId");
        
        
        User LoggedInUser = dbContext.User.FirstOrDefault(u => u.UserId == LoggedInUserId);
        
        var LoggedInUserWithTransactions = dbContext.User
        .Include(transaction => transaction.Transactions).ToList();

        ViewBag.LoggedInUser = LoggedInUser;
        
        IEnumerable<Transaction> AllTransactionsByUser =  dbContext.Transaction.Where(t => t.UserId == LoggedInUserId);
        ViewBag.AllTransactionsByUser = AllTransactionsByUser;

        return View();
    }

    [HttpPost("PostTransaction")]
    public IActionResult PostTransaction(Transaction NewTransaction)
    {
        int? LoggedInUserId = HttpContext.Session.GetInt32("UserId");   
        User UserBalanceToUpdate = dbContext.User.FirstOrDefault (u => u.UserId == LoggedInUserId);
        UserBalanceToUpdate.Balance += NewTransaction.Amount;
        dbContext.Add(NewTransaction);
        dbContext.SaveChanges();
        return RedirectToAction("ViewProfile", new { id = LoggedInUserId });
    }
  }
}
