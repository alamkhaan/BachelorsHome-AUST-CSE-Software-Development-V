using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BachelorsHome.Data;
using BachelorsHome.Models;
using System.IO;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http.Headers;

namespace BachelorsHome.Controllers
{
    public class ProductController : Controller
    {
        private readonly BachelorsHomeProductContext _context;
        private readonly BachelorsHomeContext _context2;
        private readonly IWebHostEnvironment webHostEnvironment;
        public ProductController(BachelorsHomeProductContext context, BachelorsHomeContext context2,IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _context2 = context2;
            webHostEnvironment = hostEnvironment;
        }

        // GET: Product
        public async Task<IActionResult> Index()
        {
            return View(await _context.Product.ToListAsync());
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            return View();
        }
        
        public ActionResult CommentDone(String commenttext,int Id)
        {
            CommentModel cm = new CommentModel();
            cm.UserId  = HttpContext.Session.GetInt32("userid").Value;
            cm.UserName = HttpContext.Session.GetString("username");
            var temp = _context2.UserDatabase.FirstOrDefault(m => m.Id == HttpContext.Session.GetInt32("userid").Value);
            cm.UserPic = temp.Picture;
            cm.Comment = commenttext;
            cm.CommentTime = DateTime.Now;
            cm.PostId = Id;
            cm.Editcnt = 0;
            _context.Add(cm);
            _context.SaveChanges();
            
            return RedirectToAction("ViewPost", "Product", new { @id = Id });
        }
        public JsonResult GetDistrictList(String Division)
        {
            List<SelectListItem> list2 = new List<SelectListItem>();
            var l2 = _context.DistrictModel.ToListAsync().Result;
            foreach (var item in l2)
            {
                if (item.DivisionName == Division)
                    list2.Add(new SelectListItem { Text = item.DistrictName.ToString(), Value = item.DistrictName.ToString() });
            }
            return Json(list2);

        }
        public JsonResult GetThanaList(String District)
        {
            List<SelectListItem> list3 = new List<SelectListItem>();
            var l3 = _context.ThanaModel.ToListAsync().Result;
            foreach (var item in l3)
            {
                if (item.DistrictName == District)
                    list3.Add(new SelectListItem { Text = item.ThanaName.ToString(), Value = item.ThanaName.ToString() });
            }
            return Json(list3);

        }
        public async Task<IActionResult> Advertise()
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                TempData["not-login"] = "not-login";
                return RedirectToAction("LogIn", "Account");
            }
            List<SelectListItem> list = new List<SelectListItem> ();
            var l  = await _context.DivisionModel.ToListAsync();
            foreach(var item in l)
            {
                list.Add(new SelectListItem { Text = item.DivisionName.ToString(), Value = item.DivisionName.ToString() });
            }
            ViewBag.DivisionList = list;
            List<SelectListItem> list2 = new List<SelectListItem>();
            var l2 = await _context.DistrictModel.ToListAsync();
            foreach (var item in l2)
            {
                if(item.DivisionName=="Barisal")
                    list2.Add(new SelectListItem { Text = item.DistrictName.ToString(), Value = item.DistrictName.ToString() });
            }
            ViewBag.DistrictList = list2;

            List<SelectListItem> list3 = new List<SelectListItem>();
            var l3 = await _context.ThanaModel.ToListAsync();
            foreach (var item in l3)
            {
                if (item.DistrictName == "Barguna")
                    list3.Add(new SelectListItem { Text = item.ThanaName.ToString(), Value = item.ThanaName.ToString() });
            }
            ViewBag.ThanaList = list3;
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Advertise([Bind("Id,Division,District,Thana,HomeAddress,Type,UserId,NoOfComments,RoomSize,AvailableSeat,SeatRent,TotalCost,InternetFacility,FoodFacility,AttachedWashroom,AttachedBalcony,Details,Heading,Gender,FloorNumber,AvailableFrom")]
 Product product)
        {

            
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                //Console.WriteLine("Total  file = "+files.Count);
                foreach (var Image in files)
                {
                    if (Image != null && Image.Length > 0)
                    {

                        var file = Image;
                        var uploads = Path.Combine(webHostEnvironment.WebRootPath, "Images\\Product");

                        if (file.Length > 0)
                        {
                            var fileName = ContentDispositionHeaderValue.Parse
                                (file.ContentDisposition).FileName.Trim('"');

                            
                            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                            var actual = Path.Combine(uploads, uniqueFileName);
                            var db = Path.Combine("Images\\Product", uniqueFileName);
                            //System.Console.WriteLine(actual);
                            using (var fileStream = new FileStream(actual, FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                                product.Picture = db;
                            }


                        }
                    }
                }
                product.UserId = HttpContext.Session.GetInt32("userid").Value;
                product.UserName = HttpContext.Session.GetString("username");

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }

            return View(product);
        }

        public JsonResult GetDistrictList2(String Division)
        {
            List<SelectListItem> list2 = new List<SelectListItem>();
            list2.Add(new SelectListItem { Text = "Select:", Value = "Select:" });
            var l2 = _context.DistrictModel.ToListAsync().Result;
            foreach (var item in l2)
            {
                if (item.DivisionName == Division)
                    list2.Add(new SelectListItem { Text = item.DistrictName.ToString(), Value = item.DistrictName.ToString() });
                else if (Division == "Select")
                    list2.Add(new SelectListItem { Text = item.DistrictName.ToString(), Value = item.DistrictName.ToString() });
            }
            return Json(list2);

        }
        public JsonResult GetThanaList2(String District)
        {
            List<SelectListItem> list3 = new List<SelectListItem>();
            var l3 = _context.ThanaModel.ToListAsync().Result;
            list3.Add(new SelectListItem { Text = "Select:", Value = "Select:" });
            foreach (var item in l3)
            {
                if (item.DistrictName == District)
                    list3.Add(new SelectListItem { Text = item.ThanaName.ToString(), Value = item.ThanaName.ToString() });
                else if(District=="Select")
                    list3.Add(new SelectListItem { Text = item.ThanaName.ToString(), Value = item.ThanaName.ToString() });
            }
            return Json(list3);

        }
        public async Task<IActionResult> FindHouse(SearchPost ss)
        {






            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "Select:", Value = "Select:" });
            var l = await _context.DivisionModel.ToListAsync();
            foreach (var item in l)
            {
                list.Add(new SelectListItem { Text = item.DivisionName.ToString(), Value = item.DivisionName.ToString() });
            }
            ViewBag.DivisionList = list;
            List<SelectListItem> list2 = new List<SelectListItem>();
            list2.Add(new SelectListItem { Text = "Select:", Value = "Select:" });
            var l2 = await _context.DistrictModel.ToListAsync();
            foreach (var item in l2)
            {
                list2.Add(new SelectListItem { Text = item.DistrictName.ToString(), Value = item.DistrictName.ToString() });
            }
            ViewBag.DistrictList = list2;

            List<SelectListItem> list3 = new List<SelectListItem>();
            list3.Add(new SelectListItem { Text = "Select:", Value = "Select:" });
            var l3 = await _context.ThanaModel.ToListAsync();
            foreach (var item in l3)
            {
                list3.Add(new SelectListItem { Text = item.ThanaName.ToString(), Value = item.ThanaName.ToString() });
            }
            ViewBag.ThanaList = list3;

            
            
            if(ss.AttachedWashroom==null && ss.AvailableSeat == 0 && ss.District== null && ss.Division==null && ss.Thana==null && ss.FoodFacility==null && ss.Rent==0 && ss.Username==null)
                ViewBag.ProductList = await _context.Product.ToListAsync();
            else
            {
                List<String> list4 = new List<String>();

                String main = "Select * from Product ";

                bool flag = false;
                if(ss.AttachedWashroom!=null && ss.AttachedWashroom!="Select:")
                {
                    if(flag==false)
                    {
                        flag = true;
                        main += "where AttachedWashroom='"+ss.AttachedWashroom+"'";
                    }
                    else
                    {
                        main += "and AttachedWashroom='" + ss.AttachedWashroom + "'";
                    }
                }
               

                if (ss.AvailableSeat != 0 )
                {
                    if (flag == false)
                    {
                        flag = true;
                        main += "where AvailableSeat=" + ss.AttachedWashroom;
                    }
                    else
                    {
                        main += "and AvailableSeat=" + ss.AttachedWashroom;
                    }
                }
                if (ss.District != null && ss.District != "Select:")
                {
                    if (flag == false)
                    {
                        flag = true;
                        main += "where District='" + ss.District + "'";
                    }
                    else
                    {
                        main += "and District='" + ss.District + "'";
                    }
                }
                if (ss.Division != null && ss.Division != "Select:")
                {
                    if (flag == false)
                    {
                        flag = true;
                        main += "where Division='" + ss.Division + "'";
                    }
                    else
                    {
                        main += "and Division='" + ss.Division + "'";
                    }
                }
                if (ss.Thana != null && ss.Thana != "Select:")
                {
                    if (flag == false)
                    {
                        flag = true;
                        main += "where Thana='" + ss.Thana + "'";
                    }
                    else
                    {
                        main += "and Thana='" + ss.Thana + "'";
                    }
                }

                if (ss.FoodFacility != null && ss.FoodFacility != "Select:")
                {
                    if (flag == false)
                    {
                        flag = true;
                        main += "where FoodFacility='" + ss.FoodFacility + "'";
                    }
                    else
                    {
                        main += "and FoodFacility='" + ss.FoodFacility + "'";
                    }
                }

                if (ss.Username != null && ss.Username != "Select:")
                 {
                       if (flag == false)
                        {
                            flag = true;
                            main += "where UserName='" + ss.Username + "'";
                        }
                        else
                        {
                            main += "and UserName='" + ss.Username + "'";
                        }
                 }

                if (ss.Rent != 0)
                {
                    if (flag == false)
                    {
                        flag = true;
                        main += "where TotalCost=" + ss.Rent;
                    }
                    else
                    {
                        main += "and TotalCost=" + ss.Rent;
                    }
                }

                var studentName = _context.Product.FromSqlRaw(main).ToList<Product>();
                ViewBag.ProductList = studentName;
                
               
            }
            return View();
        }
       
        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Division,District,Thana,HomeAddress,Type,UserId,NoOfComments,RoomSize,AvailableSeat,SeatRent,TotalCost,InternetFacility,FoodFacility,AttachedWashroom,AttachedBalcony,Details,Picture")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Division,District,Thana,HomeAddress,Type,UserId,NoOfComments,RoomSize,AvailableSeat,SeatRent,TotalCost,InternetFacility,FoodFacility,AttachedWashroom,AttachedBalcony,Details,Picture")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            return View(product);
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }

        public async Task<IActionResult> ViewPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (HttpContext.Session.GetInt32("userid") == null)
            {
                TempData["not-login"] = "not-login";

                return RedirectToAction("LogIn","Account");
            }
            
            var product = await _context.Product.FirstOrDefaultAsync(m => m.Id == id);
            if(product==null)
            {
                return NotFound();
            }
            
            return View(product);
        }

        public IActionResult EditComment(int ? id)
        {
            if(id==null)
            {
                return NotFound();
            }
            if (HttpContext.Session.GetInt32("userid") == null)
            {
                TempData["not-login"] = "not-login";

                return RedirectToAction("LogIn", "Account");
            }
            var c = _context.CommentModel.FirstOrDefault(m => m.Id == id);
            if(c==null)
            {
                return NotFound();
            }
            if(c.UserId!= HttpContext.Session.GetInt32("userid").Value)
            {
                return NotFound();
            }
            return View(c);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditComment(int? id,CommentModel cm)
        {
            if(id==null)
            {
                return NotFound();
            }
            int _id = 0;
            if (ModelState.IsValid)
            {
                var p = _context.CommentModel.FirstOrDefault(m => m.Id == cm.Id);
                p.Comment = cm.Comment;
                p.Editcnt++;
                _context.Update(p);
                await _context.SaveChangesAsync();
                _id = p.PostId;
                
            }
            return RedirectToAction("ViewPost", "Product", new { @id = _id });
        }

        public IActionResult DeleteComment(int ? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (HttpContext.Session.GetInt32("userid") == null)
            {
                TempData["not-login"] = "not-login";

                return RedirectToAction("LogIn", "Account");
            }
            var c = _context.CommentModel.FirstOrDefault(m => m.Id == id);
            if (c == null)
            {
                return NotFound();
            }
            if (c.UserId != HttpContext.Session.GetInt32("userid").Value)
            {
                return NotFound();
            }
            _context.CommentModel.Remove(c);
            _context.SaveChanges();
            return RedirectToAction("ViewPost", "Product", new { @id = c.PostId });
        }


        public IActionResult EditPost(int ? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var post = _context.Product.FirstOrDefault(m => m.Id == Id);
            if(post==null || HttpContext.Session.GetInt32("userid") == null || HttpContext.Session.GetInt32("userid").Value!=post.UserId)
            {
                return NotFound();
            }
            List<SelectListItem> list = new List<SelectListItem>();
            var l =  _context.DivisionModel.ToList();
            foreach (var item in l)
            {
                list.Add(new SelectListItem { Text = item.DivisionName.ToString(), Value = item.DivisionName.ToString() });
            }
            ViewBag.DivisionList = list;
            List<SelectListItem> list2 = new List<SelectListItem>();
            var l2 =  _context.DistrictModel.ToList();
            foreach (var item in l2)
            {
                if (item.DivisionName == post.Division)
                    list2.Add(new SelectListItem { Text = item.DistrictName.ToString(), Value = item.DistrictName.ToString() });
            }
            ViewBag.DistrictList = list2;

            List<SelectListItem> list3 = new List<SelectListItem>();
            var l3 = _context.ThanaModel.ToList();
            foreach (var item in l3)
            {
                if (item.DistrictName == post.District)
                    list3.Add(new SelectListItem { Text = item.ThanaName.ToString(), Value = item.ThanaName.ToString() });
            }
            ViewBag.ThanaList = list3;
            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost([Bind("Id,Division,District,Thana,HomeAddress,Type,UserId,NoOfComments,RoomSize,AvailableSeat,SeatRent,TotalCost,InternetFacility,FoodFacility,AttachedWashroom,AttachedBalcony,Details,Heading,Gender,FloorNumber,AvailableFrom")]
 Product product,int ? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var post = _context.Product.FirstOrDefault(m => m.Id == Id);
            if (post == null || HttpContext.Session.GetInt32("userid") == null || HttpContext.Session.GetInt32("userid").Value != post.UserId)
            {
                return NotFound();
            }
            if (ModelState.IsValid && product.Division!=null)
            {
                var files = HttpContext.Request.Form.Files;
                Product curr = await _context.Product.FirstOrDefaultAsync(m => m.Id == Id);
                foreach (var Image in files)
                {
                    if (Image != null && Image.Length > 0)
                    {

                        var file = Image;
                        var uploads = Path.Combine(webHostEnvironment.WebRootPath, "Images\\Product");

                        if (file.Length > 0)
                        {
                            var fileName = ContentDispositionHeaderValue.Parse
                                (file.ContentDisposition).FileName.Trim('"');


                            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                            var actual = Path.Combine(uploads, uniqueFileName);
                            var db = Path.Combine("Images\\Product", uniqueFileName);
                            //System.Console.WriteLine(actual);
                            using (var fileStream = new FileStream(actual, FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                                curr.Picture = db;
                            }


                        }
                    }
                }

                curr.AttachedBalcony = product.AttachedBalcony;
                curr.AttachedWashroom = product.AttachedWashroom;
                curr.AvailableFrom = product.AvailableFrom;
                curr.AvailableSeat = product.AvailableSeat;
                curr.Details = product.Details;
                curr.District = product.District;
                curr.Division = product.Division;
                curr.FloorNumber = product.Division;
                curr.FoodFacility = product.FoodFacility;
                curr.Gender = product.Gender;
                curr.Heading = product.Heading;
                curr.InternetFacility = product.InternetFacility;
                curr.RoomSize = product.RoomSize;
                curr.SeatRent = product.SeatRent;
                curr.Thana = product.Thana;
                curr.TotalCost = product.TotalCost;
                curr.Type = product.Type;
                curr.UserName = HttpContext.Session.GetString("username");

                _context.Update(curr);
                await _context.SaveChangesAsync();
                return RedirectToAction("ViewPost", "Product", new { @id = Id });

            }
            else
            {
                List<SelectListItem> list = new List<SelectListItem>();
                var l = _context.DivisionModel.ToList();
                foreach (var item in l)
                {
                    list.Add(new SelectListItem { Text = item.DivisionName.ToString(), Value = item.DivisionName.ToString() });
                }
                ViewBag.DivisionList = list;
                List<SelectListItem> list2 = new List<SelectListItem>();
                var l2 = _context.DistrictModel.ToList();
                foreach (var item in l2)
                {
                    if (item.DivisionName == post.Division)
                        list2.Add(new SelectListItem { Text = item.DistrictName.ToString(), Value = item.DistrictName.ToString() });
                }
                ViewBag.DistrictList = list2;

                List<SelectListItem> list3 = new List<SelectListItem>();
                var l3 = _context.ThanaModel.ToList();
                foreach (var item in l3)
                {
                    if (item.DistrictName == post.District)
                        list3.Add(new SelectListItem { Text = item.ThanaName.ToString(), Value = item.ThanaName.ToString() });
                }
                ViewBag.ThanaList = list3;
                return View(post);
            }

            
        }
        public IActionResult DeletePost(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var post = _context.Product.FirstOrDefault(m => m.Id == Id);
            if (post == null || HttpContext.Session.GetInt32("userid") == null || HttpContext.Session.GetInt32("userid").Value != post.UserId)
            {
                return NotFound();
            }
            _context.Product.Remove(post);
            _context.SaveChanges();

            return RedirectToAction("FindHouse", "Product");


        }








        public async Task<IActionResult> Chat(int ? id,Chat chat)
        {
            if(id==null)
            {
                return NotFound();
            }
            if(HttpContext.Session.GetInt32("userid") == null)
            {
                TempData["not-login"] = "not-login";
                return RedirectToAction("LogIn", "Account");

            }
            var xx =  await _context2.UserDatabase.FirstOrDefaultAsync(m=> m.Id==id);
            if(xx==null)
            {
                return NotFound();
            }
            Chat chat1 = new Chat();
            if(chat.Text!=null)
            {
                chat1.UserId = HttpContext.Session.GetInt32("userid").Value;
                chat1.OppoId = id.Value;
                chat1.Text = chat.Text;
                _context.Chat.Add(chat1);
                _context.SaveChanges();
            }
            var uniqueUserList = _context.Chat.Where(e=>e.UserId== HttpContext.Session.GetInt32("userid").Value).ToList();
            List<UserDatabase> userList = new List<UserDatabase>();
            List<int> list = new List<int>();
            foreach(var x in uniqueUserList)
            {
                list.Add(x.OppoId);
            }
            var list2 = list.Distinct();
            foreach(var i in list2)
            {
                userList.Add(_context2.UserDatabase.FirstOrDefault(m => m.Id == i));
            }
            ViewBag.ChatList = userList;


            var list4 = _context.Chat.Where(m => (m.UserId == HttpContext.Session.GetInt32("userid").Value && m.OppoId == id)|| ( m.OppoId == HttpContext.Session.GetInt32("userid").Value && m.UserId == id)) .ToList();
            Chat ch = new Chat();
            ch.OppoId = id.Value;
            ch.OppoPic = xx.Picture;
            ch.OppoName = xx.Name;
            ch.UserId = HttpContext.Session.GetInt32("userid").Value;
            ch.UserName = HttpContext.Session.GetString("username");
            ch.Text = null;
            list4.Reverse();
            foreach(var ppp in list4)
            {
                var temp = _context2.UserDatabase.FirstOrDefault(m => m.Id == ppp.UserId);
                ppp.UserName = temp.Name;
                ppp.UserPic = temp.Picture;
                temp = _context2.UserDatabase.FirstOrDefault(m => m.Id == ppp.OppoId);
                ppp.OppoName = temp.Name;
                ppp.OppoPic = temp.Picture;


            }
            ViewBag.MessageList = list4;
            return View(ch);
        }





        public async Task<IActionResult> Reviews()
        {
            if (HttpContext.Session.GetInt32("userid") == null)
            {
                TempData["not-login"] = "not-login";
                return RedirectToAction("LogIn", "Account");

            }
            var list = _context.Reviews.ToList();
            List<ReviewModel> l = new List<ReviewModel>();
            foreach (var item in list)
            {
                var temp = _context2.UserDatabase.Find(item.UserId);
                item.UserName = temp.Name;
                item.UserPic = temp.Picture;
                temp = _context2.UserDatabase.Find(item.ReviewerId);
                item.ReviewerName = temp.Name;
                item.ReviewerPic = temp.Picture;
                l.Add(item);
            }
            ViewBag.ReviewList = l;
            return View();
        }
        [HttpPost]
        public IActionResult Reviews(int SearchId)
        {
            if (HttpContext.Session.GetInt32("userid") == null)
            {
                TempData["not-login"] = "not-login";
                return RedirectToAction("LogIn", "Account");

            }
            if (SearchId == null)
            {
                return NotFound();
            }
            var p = _context2.UserDatabase.Find(SearchId);

            if (p == null)
            {
                return NotFound();
            }
            var list = _context.Reviews.Where(m => m.UserId == SearchId).ToList();
            List<ReviewModel> l = new List<ReviewModel>();
            foreach (var item in list)
            {
                var temp = _context2.UserDatabase.Find(item.UserId);
                item.UserName = temp.Name;
                item.UserPic = temp.Picture;
                temp = _context2.UserDatabase.Find(item.ReviewerId);
                item.ReviewerName = temp.Name;
                item.ReviewerPic = temp.Picture;
                l.Add(item);
            }
            ViewBag.ReviewList = l;
            return View();
        }

        public IActionResult GiveReviews(int userid,String text,double rating)
        {
            var temp = _context2.UserDatabase.FirstOrDefault(m => m.Id == userid);
            
            if(temp==null)
            {
                TempData["invalid4"] = "invalid4";
                return RedirectToAction("Reviews", "Product");
            }
            if(userid== HttpContext.Session.GetInt32("userid"))
            {
                TempData["invalid5"] = "invalid2";
                return RedirectToAction("Reviews", "Product");
            }
            var temp2 = _context.Reviews.FirstOrDefault(m => m.UserId == userid  && m.ReviewerId== HttpContext.Session.GetInt32("userid"));
            if(temp2!=null)
            {
                TempData["invalid6"] = "invalid3";
                return RedirectToAction("Reviews", "Product");
            }
            ReviewModel r = new ReviewModel();
            r.ReviewerId = HttpContext.Session.GetInt32("userid").Value;
            r.Value = rating;
            r.UserId = userid;
            r.Text = text;

            _context.Reviews.Add(r);
            _context.SaveChanges();
            TempData["invalid7"] = "invalid3";

            return RedirectToAction("Reviews", "Product");
        }




    }
}
