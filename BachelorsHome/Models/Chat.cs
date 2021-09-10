using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BachelorsHome.Models
{
    public class Chat
    {
        public int Id { get; set; }

        public String Text { get; set; }

        public int UserId { get; set; }

        public String UserName { get; set; }

        public String UserPic { get; set; }


        public int OppoId { get; set; }

        public String OppoName { get; set; }

        public String OppoPic{get;set;}
    }
}
