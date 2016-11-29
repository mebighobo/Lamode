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
            //This piece of code inject country name into the database and has to be run just one time
            //lamodeEntities db = new lamodeEntities();
            //int i = 1;
            //foreach (CultureInfo ci in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
            //{
            //    var country = new RegionInfo(new CultureInfo(ci.Name, false).LCID);
            //    string ri1 = country.DisplayName.ToString();
            //    Country country1 = new Country();
            //    country1.CountryName = ri1;
            //    country1.CountryId = i;
            //    db.Countries.Add(country1);
            //    db.SaveChanges();
            //    i++;

            //}
           
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
                    lamodeEntities context = new lamodeEntities();
                    var user = context.AspNetUsers.Where(u => u.UserName == login.UserName).FirstOrDefault();
                    var role = manager.GetRoles(user.Id);

                    if (role[0] == "Admin")
                    {
                        return RedirectToAction("AdminOnly", "Home");
                    }
                    else if (role[0] == "VIPUser")
                    {
                        return RedirectToAction("VIPUser", "Home");
                    }
                    else if (role[0] == "SpecialUser")
                    {
                        return RedirectToAction("SpecialUser", "Home");
                    }


                }
                return RedirectToAction("SecureArea", "Home");
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

            //this gets current country from user
            //additionalUserInfo.Nationality = newUser.Nationality;
            //string country = RegionInfo.CurrentRegion.DisplayName;
            //ViewBag.country = country; 
            additionalUserInfo.Country = newUser.country;
            additionalUserInfo.City = newUser.City;
            additionalUserInfo.Province = newUser.state;
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

        [HttpGet]
        public ActionResult AddRole()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddRole(AspNetRole role)
        {
            lamodeEntities context = new lamodeEntities();
            context.AspNetRoles.Add(role);
            context.SaveChanges();
            return View();
        }

        [HttpGet]
        public ActionResult AddUserToRole()
        {
            lamodeEntities context = new lamodeEntities();
            AspNetRole aspNetRole = new AspNetRole();
            AspNetUser aspNetUser = new AspNetUser();
            var list = context.AspNetRoles.ToList();
            var listUser = context.AspNetUsers.ToList();
            ViewBag.RoleList = list.ToList();
            ViewBag.UserList = listUser.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult AddUserToRole(string userName, string Id)
        {
            lamodeEntities context = new lamodeEntities();

            AspNetUser user = context.AspNetUsers
                             .Where(u => u.UserName == userName).FirstOrDefault();
            AspNetRole role = context.AspNetRoles
                             .Where(r => r.Id == Id).FirstOrDefault();

                user.AspNetRoles.Add(role);
                context.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        // To allow more than one role access use syntax like the following:
        // [Authorize(Roles="Admin, Staff")]
        public ActionResult AdminOnly()
        {
            return View();
        }

        [Authorize(Roles = "VIPUser")]
        // To allow more than one role access use syntax like the following:
        // [Authorize(Roles="Admin, Staff")]
        public ActionResult VIPUser()
        {
            return View();
        }
        [Authorize(Roles = "SpecialUser")]
        // To allow more than one role access use syntax like the following:
        // [Authorize(Roles="Admin, Staff")]
        public ActionResult SpecialUser()
        {
            return View();
        }
        [Authorize(Roles = "User")]
        // To allow more than one role access use syntax like the following:
        // [Authorize(Roles="Admin, Staff")]
        public ActionResult NormalUser()
        {
            return RedirectToAction("SecureArea");
        }




    }
}

