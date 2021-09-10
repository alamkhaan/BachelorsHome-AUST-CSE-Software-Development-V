using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BachelorsHome.Models;

namespace BachelorsHome.Data
{
    public class BachelorsHomeContext : DbContext
    {
        public BachelorsHomeContext (DbContextOptions<BachelorsHomeContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<BachelorsHome.Models.UserDatabase> UserDatabase { get; set; }

        


    }
}
