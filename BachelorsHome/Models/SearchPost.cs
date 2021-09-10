using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BachelorsHome.Models
{
    public class SearchPost
    {
        public String Username { get; set; }

        public String Type { get; set; }

        public String Division { get; set; }
        public String District { get; set; }
        public String Thana { get; set; }
        public int Rent { get; set; }

        public int AvailableSeat { get; set; }

        public String AttachedWashroom { get; set; }

        public String FoodFacility { get; set; }


    }
}
