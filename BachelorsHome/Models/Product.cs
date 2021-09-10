using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BachelorsHome.Models
{
    public class Product
    {
        public int Id { get; set; }

        public String Division { get; set; }

        public String District { get; set; }

        public String Thana { get; set; }

        public String HomeAddress { get; set; }

        public String Type { get; set; }

        public int UserId { get; set; }
        public String UserName { get; set; }
        public int NoOfComments { get; set; }

        public String RoomSize { get; set; }

        public int AvailableSeat { get; set; }

        public int SeatRent { get; set; }

        public int TotalCost { get; set; }

        public String InternetFacility { get; set; }

        public String FoodFacility { get; set; }

        public String AttachedWashroom { get; set; }

        public String AttachedBalcony { get; set; }

        public String Details { get; set; }

        public String Picture { get; set; }

        public String Heading { get; set; }

        public DateTime? AvailableFrom { get; set; }

        public String FloorNumber { get; set; }

        public String Gender { get; set; }

    }
}
