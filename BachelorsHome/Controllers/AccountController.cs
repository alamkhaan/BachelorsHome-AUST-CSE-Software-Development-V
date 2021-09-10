using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BachelorsHome.Data;
using BachelorsHome.Models;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http.Headers;
using System.IO;
using System.Web;
//using MimeKit;
//using MailKit.Net.Smtp;


using System.Net.Mail;
using MimeKit;

namespace BachelorsHome.Controllers
{
    public class AccountController : Controller
    {
        private readonly BachelorsHomeContext _context;
        private readonly BachelorsHomeProductContext _context2;
        private readonly IWebHostEnvironment webHostEnvironment;
        public AccountController(BachelorsHomeContext context, BachelorsHomeProductContext context2, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _context2 = context2;
            webHostEnvironment = hostEnvironment;
        }

        // GET: Account
        public async Task<IActionResult> Index()
        {
            return View(await _context.UserDatabase.ToListAsync());
        }

        // GET: Account/Details/5

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userDatabase = await _context.UserDatabase
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userDatabase == null)
            {
                return NotFound();
            }

            return View(userDatabase);
        }

        // GET: Account/Create
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword([Bind("Email")] UserDatabase user)
        {
            var userData = await _context.UserDatabase.FirstOrDefaultAsync(e => e.Email == user.Email);
            if (userData == null)
            {
                ViewBag.Notification = "Invalid";
            }
            else
            {
                

                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress("bachelorshome.aust@gmail.com");
                message.To.Add(new MailAddress(user.Email));
                message.Subject = "Password Recovery for Bachelors Home";
                message.IsBodyHtml = true; //to make message body as html  

                String htmlString = @"<h1>Hello "+userData.Name+@"!</h1>" + @"<p>Thank You for taking our password recovery feature.</p>" + @"<p>We are always there to serve you.</p><br/>" + @"<p>Your Password is: "+userData.Password+@"</p>" + @"<h4>Sincerely Yours,</h4>" + @"<p>- Team Bachelors Home</p>";
                message.Body = htmlString;
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com"; //for gmail host  
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential("bachelorshome.aust@gmail.com", "bachelors@1234");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
                ViewBag.Notification = "Success";

            }
                
            return View(userData);
        }
        public async Task<IActionResult> Profile(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userDatabase = await _context.UserDatabase.FirstOrDefaultAsync(m => m.Id == id);
            if (userDatabase == null)
            {
                return NotFound();
            }
            var uniqueUserList = _context2.Chat.Where(e => e.UserId == id).ToList();
            List<UserDatabase> userList = new List<UserDatabase>();
            List<int> list = new List<int>();
            foreach (var x in uniqueUserList)
            {
                list.Add(x.OppoId);
            }
            var list2 = list.Distinct();
            userDatabase.Connections = list2.Count();

            userDatabase.NumberOfPosts = _context2.Product.ToList().Count();

            var temp9 = _context2.Reviews.Where(m => m.UserId == id).ToList();
            userDatabase.NumberOfReviews = temp9.Count;
            double cnt = 0;
            double sum = 0;
            foreach(var item in temp9)
            {
                sum += item.Value;
                cnt += 1.0;
            }
            if (cnt != 0)
                userDatabase.Rating = sum / cnt;
            else userDatabase.Rating = 0.000;
            userDatabase.Rating = Math.Round(userDatabase.Rating, 2);
            return View(userDatabase);
        }
        

        
        public async Task<IActionResult> EditProfile([Bind("Name,Email,Password,Phone,Profession,CurrentAddress,PermanentAddress,AboutMe,Facebook,Hobby,ProfessionalInterest,ReligiousViews,PoliticalInterest")] UserDatabase userDatabase, int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.UserDatabase.FirstOrDefaultAsync(m => m.Id == id);

            if (HttpContext.Session.GetInt32("userid") == null || user==null)
            {
                return NotFound();
            }
            if (HttpContext.Session.GetInt32("userid") != id)
            {
                return RedirectToAction("Profile", "Account", new { @id = id });
            }
            if (ModelState.IsValid && userDatabase.Name!=null)
            {
                if (userDatabase.Password.Length < 6)
                {
                    ViewBag.Notification = "Password";
                    return View(userDatabase);
                }

                var userbase = await _context.UserDatabase.FirstOrDefaultAsync(e => e.Email == userDatabase.Email);

                if (userbase != null && user.Email!= userDatabase.Email)
                {
                    ViewBag.Notification = "Duplicate";
                    return View(userDatabase);
                }
                var files = HttpContext.Request.Form.Files;
                
                foreach (var Image in files)
                {
                    if (Image != null && Image.Length > 0)
                    {

                        var file = Image;
                        var uploads = Path.Combine(webHostEnvironment.WebRootPath, "Images\\User");

                        if (file.Length > 0)
                        {
                            var fileName = ContentDispositionHeaderValue.Parse
                                (file.ContentDisposition).FileName.Trim('"');


                            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                            var actual = Path.Combine(uploads, uniqueFileName);
                            var db = Path.Combine("Images/User", uniqueFileName);
                            
                            using (var fileStream = new FileStream(actual, FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                                user.Picture = "/"+db;
                            }


                        }
                    }
                }
                user.AboutMe = userDatabase.AboutMe;
                user.CurrentAddress = userDatabase.CurrentAddress;
                user.PermanentAddress = userDatabase.PermanentAddress;
                user.Email = userDatabase.Email;
                user.Facebook = userDatabase.Facebook;
                user.Hobby = userDatabase.Hobby;
                user.Name = userDatabase.Name;
                user.Password = userDatabase.Password;
                user.Phone = userDatabase.Phone;
                user.PoliticalInterest = userDatabase.PoliticalInterest;
                user.Profession = userDatabase.Profession;
                user.ProfessionalInterest = userDatabase.ProfessionalInterest;
                user.ReligiousViews = userDatabase.ReligiousViews;
                
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserDatabaseExists(userDatabase.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Profile", "Account", new { @id = id });
            }
            return View(user);
        }
        public IActionResult LogIn()
        {
            if (HttpContext.Session.GetString("username") != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public IActionResult LogOut()
        {
            
            HttpContext.Session.Remove("username");
            HttpContext.Session.Clear();
            TempData["ErrorMessage"] = "Logged Out Successfully";
            return RedirectToAction("LogIn");
        }
        public IActionResult Registration()
        {
            if (HttpContext.Session.GetString("username") != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

            // POST: Account/Create
            // To protect from overposting attacks, enable the specific properties you want to bind to, for 
            // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration([Bind("Id,Name,Email,Password,Phone,Profession,CurrentAddress,Picture,Rating,Gender,BloodGroup,DateofBirth")] UserDatabase userDatabase)
        {
            if (ModelState.IsValid)
            {

                if(userDatabase.Password.Length<6)
                {
                    ViewBag.Notification = "Password";
                    return View(userDatabase);
                }

                var userbase = await _context.UserDatabase.FirstOrDefaultAsync(e => e.Email == userDatabase.Email);

                if (userbase != null)
                {
                    ViewBag.Notification = "Duplicate";
                    return View(userDatabase);
                }
                userDatabase.AboutMe = "";
                userDatabase.Connections = 0;
                userDatabase.Facebook = "";
                userDatabase.Hobby = "";
                userDatabase.NumberOfPosts = 0;
                userDatabase.NumberOfReviews = 0;
                userDatabase.Rating = 0.0;
                userDatabase.Picture = "/Images/user.jpg";
                userDatabase.RegistrationDate = DateTime.Now;
                _context.Add(userDatabase);
                await _context.SaveChangesAsync();
                TempData["ErrorMessage"] = "Account Created Successfully";
                return RedirectToAction(nameof(LogIn));
            }
            return View(userDatabase);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn([Bind("Email,Password")] UserDatabase userDatabase)
        {
            if (ModelState.IsValid)
            {
                var userbase = await _context.UserDatabase.FirstOrDefaultAsync(e => e.Email == userDatabase.Email &&  e.Password== userDatabase.Password);
                
                if (userbase == null)
                {
                    ViewBag.Notification = "Error";
                    return View();
                }
                
                HttpContext.Session.SetString("username", userbase.Name);
                HttpContext.Session.SetInt32("userid", userbase.Id);
                return RedirectToAction("Index" ,"Home");
            }
            return View(userDatabase);
        }
        // GET: Account/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userDatabase = await _context.UserDatabase.FindAsync(id);
            if (userDatabase == null)
            {
                return NotFound();
            }
            return View(userDatabase);
        }

        // POST: Account/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Password,DateofBirth,Phone,Profession,Address,Picture,Rating")] UserDatabase userDatabase)
        {
            if (id != userDatabase.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userDatabase);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserDatabaseExists(userDatabase.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(userDatabase);
        }

        // GET: Account/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userDatabase = await _context.UserDatabase
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userDatabase == null)
            {
                return NotFound();
            }

            return View(userDatabase);
        }

        // POST: Account/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userDatabase = await _context.UserDatabase.FindAsync(id);
            _context.UserDatabase.Remove(userDatabase);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            
        }

        private bool UserDatabaseExists(int id)
        {
            return _context.UserDatabase.Any(e => e.Id == id);
        }
    }
}
