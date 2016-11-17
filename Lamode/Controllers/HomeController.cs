using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Lamode.ViewModels;
using System.Data.Entity.Validation;
using System.Globalization;

namespace Lamode.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Login login)
        {
            // UserStore and UserManager manages data retreival.
            UserStore<IdentityUser> userStore = new UserStore<IdentityUser>();
            UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userStore);
            IdentityUser identityUser = manager.Find(login.UserName,
                                                             login.Password);

            if (ModelState.IsValid)
            {
                if (identityUser != null)
                {
                    IAuthenticationManager authenticationManager
                                           = HttpContext.GetOwinContext().Authentication;
                    authenticationManager
                   .SignOut(DefaultAuthenticationTypes.ExternalCookie);

                    var identity = new ClaimsIdentity(new[] {
                                            new Claim(ClaimTypes.Name, login.UserName),
                                        },
                                        DefaultAuthenticationTypes.ApplicationCookie,
                                        ClaimTypes.Name, ClaimTypes.Role);
                    // SignIn() accepts ClaimsIdentity and issues logged in cookie. 
                    authenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = false
                    }, identity);
                    return RedirectToAction("SecureArea", "Home");
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult BeforeRegister()
        {
            return View();
        }
       

        [HttpGet]
        public ActionResult Register(string registeredPeople,string oneTwo)
        {
            string country = RegionInfo.CurrentRegion.DisplayName;
            ViewBag.country = country;
            ViewBag.registeredPeople = registeredPeople;
            ViewBag.oneTwo = oneTwo;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisteredUser newUser, string registeredPeople)
        {
            ViewBag.registeredPeople = registeredPeople;
            var userStore = new UserStore<IdentityUser>();
            var manager = new UserManager<IdentityUser>(userStore);
            var identityUser = new IdentityUser()
            {
                UserName = newUser.UserName,
                Email = newUser.Email
            };
            
            IdentityResult result = manager.Create(identityUser, newUser.Password);
            lamodeEntities db = new lamodeEntities();
          
            if (result.Succeeded)
            {
                var authenticationManager
                                  = HttpContext.Request.GetOwinContext().Authentication;
                var userIdentity = manager.CreateIdentity(identityUser,
                                           DefaultAuthenticationTypes.ApplicationCookie);
                authenticationManager.SignIn(new AuthenticationProperties() { },
                                             userIdentity);
            }
            var user = manager.Users.FirstOrDefault(u => u.UserName == newUser.UserName);
            //for the rest of data from AspNetUser table
            AdditionalUserInfo additionalUserInfo = new AdditionalUserInfo();
            additionalUserInfo.Id = user.Id;
           
            additionalUserInfo.CompanyName = newUser.CompanyName;
            
            additionalUserInfo.DateOfBirth = newUser.DateOfBirth;
            
           
            additionalUserInfo.Nationality = newUser.Nationality;
            string country = RegionInfo.CurrentRegion.DisplayName;
            ViewBag.country = country;
        //    additionalUserInfo.Country = ViewBag.country;
            additionalUserInfo.City = newUser.City;
            additionalUserInfo.Province = newUser.Province;
            additionalUserInfo.TellUsMore = newUser.TellUsMore;
            
            additionalUserInfo.Website = newUser.Website;
           
            additionalUserInfo.ZipCode = newUser.ZipCode;
            additionalUserInfo.Gender = newUser.Gender;
            
            try
            {
                db.AdditionalUserInfoes.Add(additionalUserInfo);
                db.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        System.Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }

            return RedirectToAction("MoreRegisterationForIndividuals", newUser);
        }

        public ActionResult MoreRegisterationForIndividuals(RegisteredUser newUser)
        {
            var userStore = new UserStore<IdentityUser>();
            var manager = new UserManager<IdentityUser>(userStore);
            var user = manager.Users.FirstOrDefault(u => u.UserName == newUser.UserName);
            AdditionalUserInfo additionalUserInfo = new AdditionalUserInfo();
            additionalUserInfo.Bust = newUser.Bust;
            additionalUserInfo.ColorEyes = newUser.ColorEyes;
           
            additionalUserInfo.Cup = newUser.Cup;
           
            additionalUserInfo.Dress = newUser.Dress;
            additionalUserInfo.Experience = newUser.Experience;
            additionalUserInfo.Height = newUser.Height;
            additionalUserInfo.Hips = newUser.Hips;
            
            additionalUserInfo.NudePhoto = newUser.NudePhoto;
            additionalUserInfo.Shoe = newUser.Shoe;
            
            additionalUserInfo.Waist = newUser.Waist;
           
            additionalUserInfo.Weight = newUser.Weight;
  
            return View();
        }

        [Authorize]
        public ActionResult SecureArea()
        {
            return View();
        }

        public ActionResult Logout()
        {
            var ctx = Request.GetOwinContext();
            var authenticationManager = ctx.Authentication;
            authenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Country()
        {
            ViewBag.Country = new[] {
        new SelectListItem() { Text = "Venezuela", Value = "1" },
        new SelectListItem() { Text = "United States", Value = "2" }
            };
            return View();
        }
        [HttpPost]
        public ActionResult StatesByCountry(string countryId)
        {
            // Filter the states by country. For example:
            lamodeEntities db = new lamodeEntities();
            var states = (from s in db.States
                          where s.CountryId == countryId
                          select new
                          {
                              id = s.CountryId,
                              state = s.Name
                          }).ToArray();

            return Json(states);
        }
    }
}

