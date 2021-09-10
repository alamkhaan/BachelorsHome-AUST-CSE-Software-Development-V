using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BachelorsHome.Models;

namespace BachelorsHome.Data
{
    public class BachelorsHomeProductContext : DbContext
    {
        public BachelorsHomeProductContext (DbContextOptions<BachelorsHomeProductContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

       

        public DbSet<BachelorsHome.Models.Product> Product { get; set; }
        public DbSet<BachelorsHome.Models.DivisionModel> DivisionModel { get; set; }

        public DbSet<BachelorsHome.Models.DistrictModel> DistrictModel { get; set; }

        public DbSet<BachelorsHome.Models.ThanaModel> ThanaModel { get; set; }

        public DbSet<BachelorsHome.Models.CommentModel> CommentModel { get; set; }

        public DbSet<BachelorsHome.Models.Chat> Chat { get; set; }

        public DbSet<BachelorsHome.Models.ReviewModel> Reviews { get; set; }
    }
}
