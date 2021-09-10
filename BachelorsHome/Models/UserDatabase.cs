using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BachelorsHome.Models
{
    public class UserDatabase
    {

       
        public int Id { get; set; }
        

        public String Name { get; set; }

        public String Email { get; set; }

        public String Password { get; set; }

        public String BloodGroup { get; set; }
        public int NumberOfPosts { get; set; }
        public int NumberOfReviews { get; set; }


        public int Connections { get; set; }
        public DateTime? DateofBirth { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public String Gender { get; set; }
        public String Phone { get; set; }

        public String Profession { get; set; }

        public String Hobby { get; set; }

        public String Facebook { get; set; }


        public String ReligiousViews { get; set; }
        public String PoliticalInterest { get; set; }
        public String CurrentAddress { get; set; }

        public String AboutMe { get; set; }


        public String ProfessionalInterest { get; set; }
        public String PermanentAddress { get; set; }
        public String Picture { get; set; }

        public Double Rating { get; set; }

    }
}
