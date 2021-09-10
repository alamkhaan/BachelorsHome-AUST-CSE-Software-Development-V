using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BachelorsHome.Data;
using BachelorsHome.Models;
using Microsoft.AspNetCore.Mvc;
namespace BachelorsHome.Controllers
{
    public class CommentViewComponent : ViewComponent
    {
        private readonly BachelorsHomeProductContext _context;
        private readonly BachelorsHomeContext _context2;
        public CommentViewComponent(BachelorsHomeProductContext context,BachelorsHomeContext context2)
        {
            _context = context;
            _context2 = context2;
        }
        public String timediff(DateTime x, DateTime y)
        {
            TimeSpan span = x.Subtract(y);
            if (span.TotalSeconds >= 0 && span.TotalSeconds <= 59)
            {
                return Convert.ToString(Math.Floor(span.TotalSeconds)) + "s";
            }
            else if (span.TotalSeconds >= 60 && span.TotalSeconds <= 3599)
            {
                int p = Convert.ToInt32(Math.Floor(span.TotalSeconds / 60.0));
                return Convert.ToString(p) + "m";
            }
            else if (span.TotalSeconds >= 3600 && span.TotalSeconds <= 80399)
            {
                int p = Convert.ToInt32(Math.Floor(span.TotalSeconds / 3600.0));
                return Convert.ToString(p) + "h";
            }
            else
            {
                int p = Convert.ToInt32(Math.Floor(span.TotalSeconds / 86400.0));
                return Convert.ToString(p) + "d";
            }

        }
        
        public async Task<IViewComponentResult> InvokeAsync(int postId)
        {
            var comment = _context.CommentModel.Where(m => m.PostId == postId).ToList();
            List<CommentModel> list = new List<CommentModel>();
            foreach (var item in comment)
            {
                item.NowTime = timediff(DateTime.Now, item.CommentTime);
                var temp = _context2.UserDatabase.FirstOrDefault(m => m.Id == item.UserId);
                item.UserName = temp.Name;
                item.UserPic = temp.Picture;
                list.Add(item);
            }
            list.Reverse();
            return await Task.FromResult((IViewComponentResult)View("Comment", list));
        }
    }
}
