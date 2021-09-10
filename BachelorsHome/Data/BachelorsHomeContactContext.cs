using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BachelorsHome.Models;

namespace BachelorsHome.Data
{
    public class BachelorsHomeContactContext : DbContext
    {
        public BachelorsHomeContactContext (DbContextOptions<BachelorsHomeContactContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<BachelorsHome.Models.Contacts> Contacts { get; set; }
        
    }
}
