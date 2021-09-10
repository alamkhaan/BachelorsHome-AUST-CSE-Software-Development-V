using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BachelorsHome.Models
{
    public class ReviewModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ReviewerId { get; set; }

        public String Text { get; set; }

        public double Value { get; set; }

        public String UserName { get; set; }


        public String ReviewerPic { get; set; }

        public String ReviewerName { get; set; }


        public String UserPic { get; set; }

    }
}
