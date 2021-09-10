using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BachelorsHome.Models
{
    public class CommentModel
    {
        public int Id { get; set; }

        public String Comment { get; set; }

        public int PostId { get; set; }
        public int UserId { get; set; }
        public int Editcnt { get; set; }
        public String UserName { get; set; }
        public String UserPic { get; set; }

        public String NowTime { get; set; }

        public DateTime CommentTime { get; set; }
    }
}
